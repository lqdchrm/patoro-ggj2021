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

namespace LostAndFound.Engine
{
    public abstract class DiscordGame<TGame, TPlayer> : AbstractGame<TGame, TPlayer>
        where TGame : DiscordGame<TGame, TPlayer>
        where TPlayer : BasePlayer<TGame, TPlayer>
    {
        protected DiscordClient client;

        protected DiscordGuild guild;

        protected DiscordChannel parentChannel;
        protected DiscordChannel helpChannel;

        protected readonly Dictionary<string, Room<TGame, TPlayer>> rooms = new Dictionary<string, Room<TGame, TPlayer>>();
        protected readonly Dictionary<ulong, TPlayer> players = new Dictionary<ulong, TPlayer>();

        public string Name { get; }
        public bool Ready { get; private set; }

        #region configurations
        public virtual bool IsEverythingCommand => false;
        #endregion

        public abstract TPlayer CreatePlayer(string userName);

        public DiscordGame(string name, DiscordClient client, DiscordGuild guild)
        {
            this.Name = name;
            this.client = client;
            this.guild = guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
        }


        public override async Task StartAsync()
        {
            await CleanupOldAsync();
            // create Channels
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
        public override async Task<TRoom> AddRoomAsync<TRoom>(TRoom room)
        {
            rooms.Add(room.Name, room);
            room.Game = this;
            room.VoiceChannel = await guild.CreateChannelAsync(room.Name, ChannelType.Voice, parentChannel);

            return room;
        }

        public IReadOnlyDictionary<string, Room<TGame, TPlayer>> Rooms => rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, TPlayer> Players => players.Values.ToDictionary(p => p.Name);

        private async Task<TPlayer> GetOrCreatePlayer(DiscordMember member)
        {
            if (!players.TryGetValue(member.Id, out var player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = CreatePlayer(member.DisplayName);
                players.Add(member.Id, player);

                player.Channel = await this.guild.CreateChannelAsync($"📜 {player.Name}", ChannelType.Text, parentChannel);

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

        protected override async void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.VoiceStateUpdated -= VoiceStateUpdated;
                client.MessageCreated -= OnMessageCreated;

                // cleanup
                await CleanupAsync();
            }
        }
    }


    #endregion
}
