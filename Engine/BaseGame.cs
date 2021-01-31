using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Net;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using LostAndFound.Engine.Events;

namespace LostAndFound.Engine
{ 
    public abstract class BaseGame : IDisposable
    {
        private DiscordClient client;
        private DiscordGuild guild;
        private DiscordChannel parentChannel;
        private DiscordChannel helpChannel;

        private readonly Dictionary<string, BaseRoom> rooms = new Dictionary<string, BaseRoom>();
        private readonly Dictionary<ulong, BasePlayer> players = new Dictionary<ulong, BasePlayer>();

        public string Name { get; }
        public bool Ready { get; private set; }

        public event EventHandler<PlayerRoomChange> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(BasePlayer player, BaseRoom oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerRoomChange()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<PlayerCommand> PlayerCommandSent;
        public void RaisePlayerCommand(PlayerCommand cmd)
        {
            PlayerCommandSent?.Invoke(this, cmd);
        }

        public BaseGame(string name, DiscordClient client, DiscordGuild guild)
        {
            this.Name = name;
            this.client = client;
            this.guild = guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
        }


        public virtual async Task StartAsync()
        {
            await CleanupOldAsync();
            await CreateDefaultChannelsAsync();
            Ready = true;
        }

        public async Task CleanupAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");

            foreach (var child in parentChannel.Children)
            {
                await child.DeleteAsync();
            }
            await parentChannel.DeleteAsync();
            Console.Error.WriteLine("[ENGINE] ... cleaned up");
        }

        public async Task CleanupOldAsync()
        {
            Console.Error.WriteLine("[ENGINE] Cleaning up ...");
            var oldGroup = guild.Channels.Values.FirstOrDefault(c => c.Name == this.Name);
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
            parentChannel = await this.guild.CreateChannelAsync(this.Name, ChannelType.Category);
            helpChannel = await this.guild.CreateChannelAsync("Help", ChannelType.Text, parentChannel);
            Console.Error.WriteLine("[ENGINE] ... default channels added");
        }

        private Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if (e.Guild.Id != this.guild.Id)
                return Task.CompletedTask;

            if (!Ready)
                return Task.CompletedTask;

            // handle user info
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            if (oldChannel == newChannel)
                return Task.CompletedTask;

            var oldRoom = (oldChannel != null && rooms.ContainsKey(oldChannel.Name)) ? rooms[oldChannel.Name] : null;
            var newRoom = (newChannel != null && rooms.ContainsKey(newChannel.Name)) ? rooms[newChannel.Name] : null;

            // Do not wait for these
            Task.Run(async () =>
            {
                var member = await guild.GetMemberAsync(e.User.Id);
                var player = await GetOrCreatePlayer(member);
                player.Room = newRoom;

                RaisePlayerChangedRoom(player, oldRoom);
            });

            return Task.CompletedTask;
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            if (e.Guild.Id != this.guild.Id)
                return;

            var member = await guild.GetMemberAsync(e.Author.Id);
            if (member == null) return;

            if (e.Channel.Parent != parentChannel)
                return;

            // if player is not bot
            if (member != client.CurrentUser)
            {
                var player = await GetOrCreatePlayer(member);

                if (player.Channel == e.Channel)
                {
                    var cmd = BuildCommandFromMessage(player, e);
                    RaisePlayerCommand(cmd);
                }
            }
        }

        private PlayerCommand BuildCommandFromMessage(BasePlayer player, MessageCreateEventArgs e)
        {
            var input = e.Message.Content.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            return new PlayerCommand()
            {
                Message = e.Message.Content,
                Player = player,
                Command = input[0].ToUpperInvariant(),
                Mentions = e.MentionedUsers.Select(u => players.GetValueOrDefault(u.Id)).Where(u => u != null).ToList(),
                Args = input.Skip(1).Where(s => !s.Contains("@")).ToList()
            };
        }

        #region Rooms Helpers
        public async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, bool visible)
            where TRoomCurrent : BaseRoom
        {
            rooms.Add(room.Name, room);
            room.Game = this;
                        
            room.VoiceChannel = await guild.CreateChannelAsync(room.Name, ChannelType.Voice, parentChannel);

            _ = SetRoomVisibility(room, visible);

            return room;
        }

        public async Task SetRoomVisibility(BaseRoom room, bool visible)
        {
            var role = guild.EveryoneRole;
            if (visible)
            {
                await room.VoiceChannel.AddOverwriteAsync(role, allow: Permissions.AccessChannels);
                room.IsVisible = true;
            }
            else
            {
                await room.VoiceChannel.AddOverwriteAsync(role, deny: Permissions.AccessChannels);
                room.IsVisible = false;
            }
        }

        public IReadOnlyDictionary<string, BaseRoom> Rooms => rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, BasePlayer> Players => players.Values.ToDictionary(p => p.Name);

        public abstract BasePlayer CreatePlayer(string userName);

        private async Task<BasePlayer> GetOrCreatePlayer(DiscordMember member)
        {
            if (!players.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = CreatePlayer(member.DisplayName);
                players.Add(member.Id, player);

                var overwrites = new DiscordOverwriteBuilder();

                player.Channel = await this.guild.CreateChannelAsync($"📜 {player.Name}", ChannelType.Text, parentChannel, overwrites: new[] { overwrites.For(guild.EveryoneRole).Deny(Permissions.AccessChannels) });
                await player.Channel.AddOverwriteAsync(member, Permissions.AccessChannels);

                player.User = member;

                Console.Error.WriteLine($"[ENGINE] ... Player {member.DisplayName} added");
                _ = NewPlayer(player).ContinueWith(t =>
                  {

                      if (t.IsFaulted)
                          Console.Error.WriteLine($"[ENGINE] Failed To Add Player {t.Exception}");
                      else
                          Console.Error.WriteLine($"[ENGINE] Player Added {player.Name}");


                  });
            }
            return player;
        }

        protected virtual Task NewPlayer(BasePlayer player) => Task.CompletedTask;

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
                client.VoiceStateUpdated -= VoiceStateUpdated;
                client.MessageCreated -= OnMessageCreated;

                // cleanup
                await CleanupAsync();
            }
        }

        #endregion
    }
}
