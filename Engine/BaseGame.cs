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
    public interface IGame: IDisposable
    {
        public Task StartAsync();
    }

    public abstract class BaseGame<TGame, TPlayer, TRoom> : IGame
        where TGame : BaseGame<TGame, TPlayer, TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer, TRoom>
        where TRoom : BaseRoom<TGame, TPlayer, TRoom>
    {
        private DiscordClient client;
        private DiscordGuild guild;
        private DiscordChannel parentChannel;
        private DiscordChannel helpChannel;

        private readonly Dictionary<string, TRoom> rooms = new Dictionary<string, TRoom>();
        private readonly Dictionary<ulong, TPlayer> players = new Dictionary<ulong, TPlayer>();

        public string Name { get; }
        public bool Ready { get; private set; }


        public event EventHandler<PlayerRoomChange<TGame, TPlayer, TRoom>> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(TPlayer player, TRoom oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerRoomChange<TGame, TPlayer, TRoom>()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<PlayerCommand<TGame, TPlayer, TRoom>> PlayerCommandSent;
        public void RaisePlayerCommand(TPlayer player, string cmd)
        {
            PlayerCommandSent?.Invoke(this, new PlayerCommand<TGame, TPlayer, TRoom>()
            {
                Player = player,
                Command = cmd
            });
        }

        #region configurations
        public virtual bool IsEverythingCommand => false;
        #endregion

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

        private async Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            if (e.Guild.Id != this.guild.Id)
                return;
            if (!Ready)
                return;
            // handle user info
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            if (oldChannel == newChannel)
                return;

            var oldRoom = (oldChannel != null && rooms.ContainsKey(oldChannel.Name)) ? rooms[oldChannel.Name] : null;
            var newRoom = (newChannel != null && rooms.ContainsKey(newChannel.Name)) ? rooms[newChannel.Name] : null;

            var member = await guild.GetMemberAsync(e.User.Id);
            var player = await GetOrCreatePlayer(member);
            player.Room = newRoom;

            RaisePlayerChangedRoom(player, oldRoom);
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
                    if (this.IsEverythingCommand)
                    {
                        RaisePlayerCommand(player, e.Message.Content.ToLowerInvariant());
                    }
                    else
                    {

                        var cmd = e.Message.Content.ToUpperInvariant();
                        if (e.Message.Content == cmd)
                        {
                            RaisePlayerCommand(player, cmd);
                        }
                        else
                        {
                            await player.Room.SendMessageAsync(player, e.Message.Content);
                        }
                    }
                }
            }
        }

        #region Rooms Helpers
        public async Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, Permissions allow = default, Permissions deney = default)
            where TRoomCurrent : TRoom
        {
            rooms.Add(room.Name, room);
            room.Game = this;
            var overaites = new DiscordOverwriteBuilder();
            overaites = overaites.For(guild.EveryoneRole);
            if (deney != default)
                overaites = overaites.Deny(deney);
            if (allow != default)
                overaites = overaites.Deny(allow);
            room.VoiceChannel = await guild.CreateChannelAsync(room.Name, ChannelType.Voice, parentChannel, overwrites: new[] { overaites });

            return room;
        }

        public IReadOnlyDictionary<string, TRoom> Rooms => rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, TPlayer> Players => players.Values.ToDictionary(p => p.Name);

        public abstract TPlayer CreatePlayer(string userName);

        private async Task<TPlayer> GetOrCreatePlayer(DiscordMember member)
        {
            if (!players.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = CreatePlayer(member.DisplayName);
                players.Add(member.Id, player);

                var overaites = new DiscordOverwriteBuilder();


                player.Channel = await this.guild.CreateChannelAsync($"📜 {player.Name}", ChannelType.Text, parentChannel, overwrites: new[] { overaites.For(guild.EveryoneRole).Deny(Permissions.AccessChannels) });
                await player.Channel.AddOverwriteAsync(member, Permissions.AccessChannels);

                player.User = member;

                await player.InitAsync();

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

        protected virtual Task NewPlayer(TPlayer player) => Task.CompletedTask;

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
