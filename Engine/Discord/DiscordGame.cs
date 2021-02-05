using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Discord
{
    public abstract class DiscordGameImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseGameImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        private readonly DiscordClient _Client;
        private readonly DiscordGuild _Guild;
        private DiscordChannel _ParentChannel;

        private Dictionary<string, DiscordChannel> _RoomNameToVoiceChannels = new Dictionary<string, DiscordChannel>();
        
        private Dictionary<ulong, TPlayer> _PlayerIdToPlayer = new Dictionary<ulong, TPlayer>();
        private Dictionary<string, DiscordMember> _PlayerNameToDiscordMember = new Dictionary<string, DiscordMember>();
        private Dictionary<string, DiscordChannel> _PlayerNameToDiscordChannel = new Dictionary<string, DiscordChannel>();

        public DiscordGameImpl(string name, DiscordClient client, DiscordGuild guild) : base(name)
        {
            this._Client = client;
            this._Guild = guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
            client.MessageUpdated += OnMessageUpdated;
        }

        public override async Task StartAsync()
        {
            await CleanupOldAsync();
            await CreateDefaultChannelsAsync();
            var member = await this._Guild.GetMemberAsync(this._Client.CurrentUser.Id);
            if (member != null)
                _ = member.ModifyAsync(x => x.Nickname = "GameMaster");
#if !DEBUG
            Say("A new game has started. Please select your channel.", true);
#endif
            await base.StartAsync();
        }

        private async Task CleanupAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");

            foreach (var child in this._ParentChannel.Children)
            {
                await child.DeleteAsync();
            }
            await this._ParentChannel.DeleteAsync();
            Console.Error.WriteLine("[ENGINE] ... cleaned up");
        }

        private async Task CreateDefaultChannelsAsync()
        {
            Console.Error.WriteLine("[ENGINE] Adding default channels ...");
            this._ParentChannel = await this._Guild.CreateChannelAsync(this.Name, ChannelType.Category);
            Console.Error.WriteLine("[ENGINE] ... default channels added");
        }

        private async Task CleanupOldAsync()
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

        public override async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, bool visible)
        {
            var channel = await this._Guild.CreateChannelAsync(room.Name, ChannelType.Voice, this._ParentChannel);
            _RoomNameToVoiceChannels.Add(room.Name, channel);
            return await base.AddRoomAsync(room, visible);
        }

        private Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e) => OnMessage(client, e.Guild, e.Author, e.Channel, e.Message);

        private Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs e) => OnMessage(client, e.Guild, e.Author, e.Channel, e.Message);

        private Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if (e.Guild.Id != this._Guild.Id)
                return Task.CompletedTask;

            if (!this.Ready)
                return Task.CompletedTask;

            // handle user info
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            var oldRoom = (oldChannel != null && this.Rooms.ContainsKey(oldChannel.Name)) ? this.Rooms[oldChannel.Name] : default;
            var newRoom = (newChannel != null && this.Rooms.ContainsKey(newChannel.Name)) ? this.Rooms[newChannel.Name] : default;

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
                else if (_PlayerNameToDiscordChannel.TryGetValue(player.NormalizedName, out DiscordChannel playerChannel))
                {
                    if (channel is not null && channel == playerChannel)
                    {
                        var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, message.Content);
                        RaiseCommand(cmd);
                    }
                }
            }
        }

        /// <summary>
        /// returns null, if player cannot be created or a cached player has no channel
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private async Task<TPlayer> GetOrCreatePlayer(DiscordMember member)
        {
            if (!this._PlayerIdToPlayer.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");

                 
                player = CreatePlayer(member.DisplayName);

                _PlayerNameToDiscordMember.Add(player.NormalizedName, member);

                var builder = new DiscordOverwriteBuilder();
                var guild = member.Guild;
                if (guild != null)
                {
                    var discordOverwriteBuilder = builder.For(guild.EveryoneRole).Deny(Permissions.AccessChannels);
                    var overwrites = new[] { discordOverwriteBuilder };
                    var channel = await guild.CreateChannelAsync($"{player.Emoji}{this.Name}", ChannelType.Text, _ParentChannel, overwrites: overwrites);
                    await channel.AddOverwriteAsync(member, Permissions.AccessChannels);
                    _PlayerNameToDiscordChannel.Add(player.NormalizedName, channel);
                }

                this._PlayerIdToPlayer.Add(member.Id, player);
                this._Players.Add(player.NormalizedName, player);
                Console.Error.WriteLine($"[ENGINE] ... Player {member.DisplayName} added");
            }
            return player;
        }

        public override async Task ShowRoom(TRoom room)
        {
            if (_RoomNameToVoiceChannels.TryGetValue(room.Name, out DiscordChannel channel))
            {
                var role = channel?.Guild.EveryoneRole;
                if (role is not null)
                {
                    await channel.AddOverwriteAsync(role, allow: Permissions.AccessChannels);
                }
            }
        }

        public override async Task HideRoom(TRoom room)
        {
            if (_RoomNameToVoiceChannels.TryGetValue(room.Name, out DiscordChannel channel))
            {
                var role = channel?.Guild.EveryoneRole;
                if (role is not null)
                {
                    await channel.AddOverwriteAsync(role, deny: Permissions.AccessChannels);
                }
            }
        }

        public override void Mute(TPlayer player)
        {
            if (_PlayerNameToDiscordMember.TryGetValue(player.NormalizedName, out DiscordMember member))
            {
                member.ModifyAsync(x => x.Muted = true);
            }
        }

        public override void Unmute(TPlayer player)
        {
            if (_PlayerNameToDiscordMember.TryGetValue(player.NormalizedName, out DiscordMember member))
            {
                member.ModifyAsync(x => x.Muted = false);
            }
        }

        public override bool SendReplyTo(TPlayer player, string msg)
        {
            if (_PlayerNameToDiscordChannel.TryGetValue(player.NormalizedName, out DiscordChannel channel))
            {
                msg = $"```css\n{msg}```";
                channel.SendMessageAsync(msg);
                return true;
            }
            return false;
        }

        public override bool SendImageTo(TPlayer player, string msg)
        {
            if (_PlayerNameToDiscordChannel.TryGetValue(player.NormalizedName, out DiscordChannel channel))
            {
                msg = $"```\n{msg}\n```";
                channel.SendMessageAsync(msg);
                return true;
            }
            return false;
        }

        public override bool SendSpeechTo(TPlayer player, string msg)
        {
            if (_PlayerNameToDiscordChannel.TryGetValue(player.NormalizedName, out DiscordChannel channel))
            {
                channel.SendMessageAsync(msg, true);
                return true;
            }
            return false;
        }

        public override void MovePlayerTo(TPlayer player, TRoom room)
        {
            base.MovePlayerTo(player, room);
            if (room is BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing> romImp)
            {
                if (_PlayerNameToDiscordChannel.TryGetValue(player.NormalizedName, out DiscordChannel channel)
                    && _PlayerNameToDiscordMember.TryGetValue(player.NormalizedName, out DiscordMember member))
                    channel.PlaceMemberAsync(member);
            }
        }

        #region IDisposable

        private bool disposedValue;
        public bool IsDisposed => this.disposedValue;

        public override void Dispose()
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
                this._Client.MessageUpdated -= OnMessageUpdated;

                // cleanup
                await CleanupAsync();
            }
        }
        #endregion
    }
}
