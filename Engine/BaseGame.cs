using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using LostAndFound.Engine.Events;

namespace LostAndFound.Engine
{ 
    public abstract class BaseGame<TGame, TRoom, TPlayer, TThing> : IGame
        where TGame: BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom: BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer: BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        private DiscordClient _Client;
        private DiscordGuild _Guild;
        private DiscordChannel _ParentChannel;
        private DiscordChannel _HelpChannel;

        private readonly Dictionary<string, TRoom> _Rooms = new Dictionary<string, TRoom>();
        private readonly Dictionary<ulong, TPlayer> _Players = new Dictionary<ulong, TPlayer>();

        public string Name { get; }
        public bool Ready { get; private set; }

        #region Events

        public event EventHandler<PlayerRoomChange<TGame, TRoom, TPlayer, TThing>> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(TPlayer player, TRoom oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerRoomChange<TGame, TRoom, TPlayer, TThing>()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<BaseCommand<TGame, TRoom, TPlayer, TThing>> CommandSent;
        public void RaiseCommand(BaseCommand<TGame, TRoom, TPlayer, TThing> cmd)
        {
            CommandSent?.Invoke(this, cmd);
        }

        #endregion

        public BaseGame(string name, DiscordClient client, DiscordGuild guild)
        {
            this.Name = name;
            this._Client = client;
            this._Guild = guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
        }


        public virtual async Task StartAsync()
        {
            await CleanupOldAsync();
            await CreateDefaultChannelsAsync();
            Ready = true;
            var member = await _Guild.GetMemberAsync(_Client.CurrentUser.Id);
            if (member != null)
                member.ModifyAsync(x => x.Nickname = "GameMaster");
            Say("A new game has started. Please select your channel.", true);
        }

        public async Task CleanupAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");

            foreach (var child in _ParentChannel.Children)
            {
                await child.DeleteAsync();
            }
            await _ParentChannel.DeleteAsync();
            Console.Error.WriteLine("[ENGINE] ... cleaned up");
        }

        public async Task CleanupOldAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");
            var oldGroup = _Guild.Channels.Values.FirstOrDefault(c => c.Name == this.Name);
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
            _ParentChannel = await this._Guild.CreateChannelAsync(this.Name, ChannelType.Category);
            _HelpChannel = await this._Guild.CreateChannelAsync("Help", ChannelType.Text, _ParentChannel);
            Console.Error.WriteLine("[ENGINE] ... default channels added");
        }

        private Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if (e.Guild.Id != this._Guild.Id)
                return Task.CompletedTask;

            if (!Ready)
                return Task.CompletedTask;

            // handle user info
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            if (oldChannel == newChannel)
                return Task.CompletedTask;

            var oldRoom = (oldChannel != null && _Rooms.ContainsKey(oldChannel.Name)) ? _Rooms[oldChannel.Name] : null;
            var newRoom = (newChannel != null && _Rooms.ContainsKey(newChannel.Name)) ? _Rooms[newChannel.Name] : null;

            // Do not wait for these
            Task.Run(async () =>
            {
                var member = await _Guild.GetMemberAsync(e.User.Id);
                var player = await GetOrCreatePlayer(member);
                player.Room = newRoom;

                RaisePlayerChangedRoom(player, oldRoom);
            });

            return Task.CompletedTask;
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Guild.Id != this._Guild.Id)
                return;

            var member = await _Guild.GetMemberAsync(e.Author.Id);
            if (member == null) return;

            if (e.Channel.Parent != _ParentChannel)
                return;

            // if player is not bot
            if (member != client.CurrentUser)
            {
                var player = await GetOrCreatePlayer(member);

                if (player.Room is null)
                {
                    player.Reply("Please join a voice channel");
                } else if (player._Channel == e.Channel)
                {
                    var cmd = new BaseCommand<TGame, TRoom, TPlayer, TThing>(player, e.Message.Content);
                    RaiseCommand(cmd);
                }
            }
        }

        public void Say(string msg, bool alsoInDefaultChannel = false)
        {
            var rooms = Rooms.Values.ToList();
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
            where TRoomCurrent : TRoom
        {
            _Rooms.Add(room.Name, room);
            room._VoiceChannel = await _Guild.CreateChannelAsync(room.Name, ChannelType.Voice, _ParentChannel);
            _ = _SetRoomVisibility(room, visible);

            return room;
        }

        internal async Task _SetRoomVisibility(BaseRoom<TGame, TRoom, TPlayer, TThing> room, bool visible)
        {
            var role = _Guild.EveryoneRole;
            if (visible)
            {
                await room._VoiceChannel.AddOverwriteAsync(role, allow: Permissions.AccessChannels);
                room.IsVisible = true;
                Say($"The new Room {room.Name} has appeared. You can switch Voice channels now.");
            }
            else
            {
                await room._VoiceChannel.AddOverwriteAsync(role, deny: Permissions.AccessChannels);
                room.IsVisible = false;
            }
        }

        public IReadOnlyDictionary<string, TRoom> Rooms => _Rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, TPlayer> Players => _Players.Values.ToDictionary(p => p.Name);

        protected abstract TPlayer CreatePlayer(string userName);

        private async Task<TPlayer> GetOrCreatePlayer(DiscordMember member)
        {
            if (!_Players.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = CreatePlayer(member.DisplayName);
                _Players.Add(member.Id, player);

                var overwrites = new DiscordOverwriteBuilder();

                player._Channel = await this._Guild.CreateChannelAsync($"{player.Emoji}{player.Name}", ChannelType.Text, _ParentChannel, overwrites: new[] { overwrites.For(_Guild.EveryoneRole).Deny(Permissions.AccessChannels) });
                await player._Channel.AddOverwriteAsync(member, Permissions.AccessChannels);

                player._User = member;

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
        public BaseThing<TGame, TRoom, TPlayer, TThing> GetThing(TPlayer sender, string token, BaseContainer<TGame, TRoom, TPlayer, TThing> other = null, bool showHelp = false)
        {
            bool helpCalculated = false;
            if (token is null) return null;

            // find in inventory
            BaseThing<TGame, TRoom, TPlayer, TThing> item = null;
            if (sender.Inventory.TryFind(token, out item)) return item;

            // find in room
            if (sender.Room.Inventory.TryFind(token, out item)) return item;

            // find in other inventory
            if (other?.Inventory.TryFind(token, out item) ?? false) return item;

            //search player
            if (token.Length > 2)
            {
                var candidates = Players.Values.Where(p => p.NormalizedName.StartsWith(token)).ToList();

                // only one match
                if (candidates.Count() == 1) return candidates.First();

                // only one exact match
                if ((item = candidates.FirstOrDefault(p => p.NormalizedName == token)) != null) return item;

                // multiple matches
                if (candidates.Count() > 1)
                {
                    var names = string.Join(", ", candidates.Select(cand => cand.Name));
                    sender.Reply($"Found multiple players: {names}");
                    helpCalculated = true;
                }
            }

            // check if current room was meant
            if (sender.Room.Name.ToLowerInvariant() == token.ToLowerInvariant()) return sender.Room;

            // show possible solutions
            if (!helpCalculated && showHelp) {
                var available = sender.Inventory.ToDictionary(i => i.Name);                     // own inventory
                if (other is not null)                                                          // other inventory
                    available = available.Merge(other.Inventory.ToDictionary(i => i.Name));
                available = available.Merge(sender.Room.Inventory.ToDictionary(i => i.Name));   // room inventory
                available = available.Merge(Players.ToDictionary(_ => _.Key, _ => _.Value));    // player names

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
        public bool IsDisposed => disposedValue;

        public void Dispose()
        {
            if (!disposedValue)
            {
                disposedValue = true;

                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Client.VoiceStateUpdated -= VoiceStateUpdated;
                _Client.MessageCreated -= OnMessageCreated;

                // cleanup
                await CleanupAsync();
            }
        }

        #endregion
    }
}
