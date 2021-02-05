using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface BaseGame<TGame, TPlayer, TRoom, TContainer, TThing> : IGame
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public IReadOnlyDictionary<string, TRoom> Rooms { get; }
        public IReadOnlyDictionary<string, TPlayer> Players { get; }

        public void Say(string msg, bool alsoInDefaultChannel = false);

        public Task ShowRoom(TRoom room);
        public Task HideRoom(TRoom room);
        public void Mute(TPlayer player);
        public void Unmute(TPlayer player);

        public bool SendReplyTo(TPlayer player, string msg);
        public bool SendReplyWithStateTo(TPlayer player, string msg);
        public bool SendImageTo(TPlayer player, string msg);
        public bool SendSpeechTo(TPlayer player, string msg);

        public void MovePlayerTo(TPlayer player, TRoom room);
    }

    public abstract class BaseGameImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public string Name { get; }
        public bool Ready { get; private set; }

        #region Events

        public event EventHandler<PlayerRoomChange<TGame, TPlayer, TRoom, TContainer, TThing>> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(TPlayer player, TRoom oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerRoomChange<TGame, TPlayer, TRoom, TContainer, TThing>()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>> CommandSent;
        public void RaiseCommand(BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing> cmd) => CommandSent?.Invoke(this, cmd);

        #endregion

        public BaseGameImpl(string name)
        {
            this.Name = name;
        }

        public virtual Task StartAsync() { this.Ready = true; return Task.CompletedTask; }

        public abstract Task ShowRoom(TRoom room);
        public abstract Task HideRoom(TRoom room);

        public abstract void Mute(TPlayer player);
        public abstract void Unmute(TPlayer player);
        public abstract bool SendReplyTo(TPlayer player, string msg);
        public virtual bool SendReplyWithStateTo(TPlayer player, string msg) => SendReplyTo(player, $"{msg}\n{player.StatusText}");
        public abstract bool SendImageTo(TPlayer player, string msg);
        public abstract bool SendSpeechTo(TPlayer player, string msg);
        public virtual void MovePlayerTo(TPlayer player, TRoom room) { player.Room = room;}

        public void Say(string msg, bool alsoInDefaultChannel = false)
        {
            var rooms = this.Rooms.Values.ToList();
            Task.Run(async () =>
            {
                foreach (var room in rooms)
                {
                    room.Say(msg);
                    await Task.Delay(150);
                }
            });

            //if (alsoInDefaultChannel)
            //    this._Guild.GetDefaultChannel().SendMessageAsync(msg, true);
        }


        protected Dictionary<string, TRoom> _Rooms = new Dictionary<string, TRoom>();
        public IReadOnlyDictionary<string, TRoom> Rooms => _Rooms;

        public virtual async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, bool visible)
            where TRoomCurrent : BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing>, TRoom
        {
            this._Rooms.Add(room.Name, room);

            if (visible)
            {
                await room.Show(true);
            }
            else
            {
                await room.Hide(true);
            }

            return room;
        }


        protected Dictionary<string, TPlayer> _Players = new Dictionary<string, TPlayer>();
        public IReadOnlyDictionary<string, TPlayer> Players => _Players;

        protected abstract TPlayer CreatePlayer(string name);


        /// <summary>
        /// search for a thing
        /// 
        /// first search in player's inventory
        /// then search room's inventory
        /// then search player by name
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public TThing GetThing(TPlayer sender, string token, TContainer other = default, bool showHelp = false)
        {
            bool helpCalculated = false;
            if (token is null) return default;

            // find in inventory
            if (sender.Inventory.TryFind(token, out TThing item)) return item;

            // find in room
            if (sender.Room.Inventory.TryFind(token, out item)) return item;

            // find in other inventory
            if (other?.Inventory.TryFind(token, out item) ?? false) return item;

            // find in all containers in room to show some help
            if (showHelp)
            {
                var containerContainingToken = sender.Room?.Inventory.OfType<TContainer>()
                    .FirstOrDefault(c => c.Inventory.Any(i => i.Name.ToLowerInvariant().Equals(token?.ToLowerInvariant())));
                if (containerContainingToken != null)
                {
                    sender.Reply($"There is no {token} here. Maybe you should try it with {containerContainingToken}."); // first thing not found
                }
                return default;
            }

            //search player
            if (token.Length > 2)
            {
                var candidates = sender.Room.Players.Where(p => p.NormalizedName.StartsWith(token)).ToList();

                // only one match
                if (candidates.Count == 1) return candidates.First();

                // only one exact match
                if ((item = candidates.FirstOrDefault(p => p.NormalizedName == token)) != null) return item;

                // multiple matches
                if (candidates.Count > 1)
                {
                    var names = string.Join(", ", candidates.Select(cand => cand.Name));
                    sender.Reply($"Found multiple players: {names}");
                    helpCalculated = true;
                }
            }

            // check if current room was meant
            if (sender.Room.Name.ToLowerInvariant() == token.ToLowerInvariant())
                return sender.Room;

            // show possible solutions
            if (!helpCalculated && showHelp)
            {
                var available = sender.Inventory.ToDictionary(i => i.Name);                     // own inventory
                if (other is not null)                                                          // other inventory
                    available = available.Merge(other.Inventory.ToDictionary(i => i.Name));
                available = available.Merge(sender.Room.Inventory.ToDictionary(i => i.Name));   // room inventory
                available = available.Merge(sender.Room.Players.ToDictionary(_ => _.NormalizedName));    // player names

                var distances = available.Keys.Select(key => (key, value: token.Levenshtein(key)));
                var toShow = distances.OrderBy(dist => dist.value).Take(3);

                var helpText = $"Did you mean one of these: " + string.Join(", ", toShow.Select(kvp => available[kvp.key]));
                sender.Reply(helpText);
            }

            return item;
        }

        public abstract void Dispose();
    }
}
