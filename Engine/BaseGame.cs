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
        string Name { get; }

        IReadOnlyDictionary<string, TRoom> Rooms { get; }
        IReadOnlyDictionary<string, TPlayer> Players { get; }

        void RaisePlayerChangedRoom(TPlayer player, TRoom oldRoom);
        void RaiseCommand(BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing> cmd);
        TPlayer CreateAndAddPlayer(string name);

        #region Engine delegation
        string FormatThing(TThing thing);
        Task ShowRoom(TRoom room);
        Task HideRoom(TRoom room);
        void Mute(TPlayer player);
        void Unmute(TPlayer player);
        void Say(string msg);
        void BroadcastMsg(string msg, params TPlayer[] excluded);
        void SendMsgTo(TPlayer player, string msg);
        void SendMsgWithStateTo(TPlayer player, string msg);
        void SendImageTo(TPlayer player, string msg);
        void SendSpeechTo(TPlayer player, string msg);
        void MovePlayerTo(TPlayer player, TRoom room);
        #endregion
    }

    public abstract class BaseGameImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public BaseEngine<TGame, TPlayer, TRoom, TContainer, TThing> Engine { get; }
        public string Name { get; }

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

        public BaseGameImpl(string name, BaseEngine<TGame, TPlayer, TRoom, TContainer, TThing> engine)
        {
            this.Name = name;
            this.Engine = engine;
            this.Engine.Game = this;
        }

        #region Init
        public async Task StartAsync()
        {
            await Engine.PrepareEngine();
            await InitAsync();
            await Engine.StartEngine();
        }

        public abstract Task InitAsync();
        #endregion 

        #region Engine delegation
        public string FormatThing(TThing thing) => Engine.FormatThing(thing);
        public Task ShowRoom(TRoom room) => Engine.ShowRoom(room);
        public Task HideRoom(TRoom room) => Engine.HideRoom(room);
        public void Mute(TPlayer player) => Engine.Mute(player);
        public void Unmute(TPlayer player) => Engine.Unmute(player);
        public void SendMsgTo(TPlayer player, string msg) => Engine.SendReplyTo(player, msg);
        public void SendMsgWithStateTo(TPlayer player, string msg) => SendMsgTo(player, $"{msg}\n{player.StatusText}");
        public void SendImageTo(TPlayer player, string msg) => Engine.SendImageTo(player, msg);
        public void SendSpeechTo(TPlayer player, string msg) => Engine.SendSpeechTo(player, msg);
        public void MovePlayerTo(TPlayer player, TRoom room) { player.Room = room; Engine.MovePlayerTo(player, room); }
        #endregion

        public void Say(string msg)
        {
            Task.Run(async () =>
            {
                foreach (var player in Players.Values)
                {
                    player.Say(msg);
                    await Task.Delay(50);
                }
            });
        }

        public void BroadcastMsg(string msg, params TPlayer[] excluded)
        {
            Task.Run(async () =>
            {
                foreach (var player in Players.Values.Except(excluded))
                {
                    player.Reply(msg);
                    await Task.Delay(50);
                }
            });
        }

    #region Room

    protected Dictionary<string, TRoom> _Rooms = new Dictionary<string, TRoom>();
        public IReadOnlyDictionary<string, TRoom> Rooms => _Rooms;

        public virtual async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, bool visible)
            where TRoomCurrent : BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing>, TRoom
        {
            await this.Engine.InitRoom(room);
            
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
        #endregion


        #region Player

        protected Dictionary<string, TPlayer> _Players = new Dictionary<string, TPlayer>();

        public IReadOnlyDictionary<string, TPlayer> Players => _Players;

        public abstract TPlayer CreateAndAddPlayer(string name);
        
        #endregion

        public TThing GetThing(TPlayer sender, string token, TContainer other = default, bool showHelp = false)
        {
            bool helpCalculated = false;
            if (token is null) return default;

            if (token.ToLowerInvariant() == "me")
                return sender;

            // find in inventory
            if (sender.TryFind(token, out TThing item)) return item;

            // find in room
            if (sender.Room.TryFind(token, out item)) return item;

            // find in other inventory
            if (other?.TryFind(token, out item) ?? false) return item;

            // find in all containers in room to show some help
            if (showHelp)
            {
                var containerContainingToken = sender.Room?.OfType<TContainer>()
                    .FirstOrDefault(c => c.Any(i => i.Name.ToLowerInvariant().Equals(token?.ToLowerInvariant())));
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
                var available = sender.ToDictionary(i => i.Name);                     // own inventory
                if (other is not null)                                                          // other inventory
                    available = available.Merge(other.ToDictionary(i => i.Name));
                available = available.Merge(sender.Room.ToDictionary(i => i.Name));   // room inventory
                available = available.Merge(sender.Room.Players.ToDictionary(_ => _.NormalizedName));    // player names

                var distances = available.Keys.Select(key => (key, value: token.Levenshtein(key)));
                var toShow = distances.OrderBy(dist => dist.value).Take(3);

                var helpText = $"Did you mean one of these: " + string.Join(", ", toShow.Select(kvp => available[kvp.key]));
                sender.Reply(helpText);
            }

            return item;
        }


        #region IDispoable
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Engine.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
