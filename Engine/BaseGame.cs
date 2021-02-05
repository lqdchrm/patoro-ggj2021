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
        public IReadOnlyDictionary<string, TPlayer> Players { get; }

        public void Say(string msg, bool alsoInDefaultChannel = false);
    }

    public abstract class BaseGameImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        private readonly DiscordClient _Client;
        private readonly DiscordGuild _Guild;
        internal DiscordChannel _ParentChannel;

        private readonly Dictionary<string, TRoom> _Rooms = new Dictionary<string, TRoom>();
        private readonly Dictionary<ulong, TPlayer> _Players = new Dictionary<ulong, TPlayer>();

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

        public BaseGameImpl(string name, DiscordClient client, DiscordGuild guild)
        {
            this.Name = name;
            this._Client = client;
            this._Guild = guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
        }

        public virtual async Task StartAsync()
        {
            await CleanupOldAsync();
            await CreateDefaultChannelsAsync();
            this.Ready = true;
            var member = await this._Guild.GetMemberAsync(this._Client.CurrentUser.Id);
            if (member != null)
                _ = member.ModifyAsync(x => x.Nickname = "GameMaster");
            Say("A new game has started. Please select your channel.", true);
        }

        public async Task CleanupAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");

            foreach (var child in this._ParentChannel.Children)
            {
                await child.DeleteAsync();
            }
            await this._ParentChannel.DeleteAsync();
            Console.Error.WriteLine("[ENGINE] ... cleaned up");
        }

        public async Task CleanupOldAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");
            var oldGroup = this._Guild.Channels.Values.FirstOrDefault(c => c.Name == this.Name);
            if (oldGroup != null)
            {
                foreach (var child in oldGroup.Children)
                {
                    await child.DeleteAsync();
                }
                await oldGroup.DeleteAsync();
            }
            Console.Error.WriteLine("[ENGINE] ... cleaned up");
        }

        private async Task CreateDefaultChannelsAsync()
        {
            Console.Error.WriteLine("[ENGINE] Adding default channels ...");
            this._ParentChannel = await this._Guild.CreateChannelAsync(this.Name, ChannelType.Category);
            Console.Error.WriteLine("[ENGINE] ... default channels added");
        }

        private Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if (e.Guild.Id != this._Guild.Id)
                return Task.CompletedTask;

            if (!this.Ready)
                return Task.CompletedTask;

            // handle user info
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            var oldRoom = (oldChannel != null && this._Rooms.ContainsKey(oldChannel.Name)) ? this._Rooms[oldChannel.Name] : default;
            var newRoom = (newChannel != null && this._Rooms.ContainsKey(newChannel.Name)) ? this._Rooms[newChannel.Name] : default;

            if (oldRoom == newRoom)
                return Task.CompletedTask;

            // Do not wait for these
            Task.Run(async () =>
            {
                var member = await this._Guild.GetMemberAsync(e.User.Id);
                var player = await GetOrCreatePlayer(member);
                player.Room = newRoom;

                RaisePlayerChangedRoom(player, oldRoom);
            });

            return Task.CompletedTask;
        }

        private async Task OnMessage(DiscordClient client, DiscordGuild guild, DiscordUser author, DiscordChannel channel, DiscordMessage message)
        {
            if (guild.Id != this._Guild.Id)
                return;

            var member = await this._Guild.GetMemberAsync(author.Id);
            if (member == null) return;

            if (channel.Parent != this._ParentChannel)
                return;

            // if player is not bot
            if (member != client.CurrentUser)
            {
                var player = await GetOrCreatePlayer(member);

                if (player.Room is null)
                {
                    player.Reply("Please join a voice channel");
                }
                else if (player._UsesChannel(channel))
                {
                    var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, message.Content);
                    RaiseCommand(cmd);
                }
            }
        }

        private Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e) => OnMessage(client, e.Guild, e.Author, e.Channel, e.Message);

        private Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs e) => OnMessage(client, e.Guild, e.Author, e.Channel, e.Message);

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

            if (alsoInDefaultChannel)
                this._Guild.GetDefaultChannel().SendMessageAsync(msg, true);
        }


        #region Rooms Helpers
        public async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, bool visible)
            where TRoomCurrent : BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing>, TRoom
        {
            this._Rooms.Add(room.Name, room);
            room._VoiceChannel = await this._Guild.CreateChannelAsync(room.Name, ChannelType.Voice, this._ParentChannel);
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

        public IReadOnlyDictionary<string, TRoom> Rooms => this._Rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, TPlayer> Players => this._Players.Values.ToDictionary(p => p.Name);

        protected abstract TPlayer CreatePlayer(DiscordMember member);

        /// <summary>
        /// returns null, if player cannot be created or a cached player has no channel
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private async Task<TPlayer> GetOrCreatePlayer(DiscordMember member)
        {


            if (!this._Players.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = CreatePlayer(member);
                await player._InitPlayer(this._ParentChannel);


                this._Players.Add(member.Id, player);
                Console.Error.WriteLine($"[ENGINE] ... Player {member.DisplayName} added");
            }
            return player;
        }

        #endregion

        #region Thing Helpers

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
        #endregion

        #region IDisposable

        private bool disposedValue;
        public bool IsDisposed => this.disposedValue;

        public void Dispose()
        {
            if (!this.disposedValue)
            {
                this.disposedValue = true;

                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._Client.VoiceStateUpdated -= VoiceStateUpdated;
                this._Client.MessageCreated -= OnMessageCreated;

                // cleanup
                await CleanupAsync();
            }
        }

        #endregion
    }
}
