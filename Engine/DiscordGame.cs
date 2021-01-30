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
    public abstract class DiscordGame : AbstractGame
    {
        protected DiscordClient client;
        
        protected DiscordGuild guild;

        protected DiscordChannel parentChannel;
        protected DiscordChannel helpChannel;

        protected readonly Dictionary<string, Room> rooms = new Dictionary<string, Room>();
        protected readonly Dictionary<ulong, Player> players = new Dictionary<ulong, Player>();

        public string Name { get; }
        public bool Ready { get; private set; }

        public DiscordGame(string name) { this.Name = name; }

        public override async Task StartAsync()
        {
            DotNetEnv.Env.Load();

            Console.Error.WriteLine("[ENGINE] Creating Discord client ...");
            client = new DiscordClient(new DiscordConfiguration
            {
                Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
            });
            Console.Error.WriteLine("[ENGINE] ... Discord client created");

            Console.Error.WriteLine("[ENGINE] Adding Handlers ...");
            client.GuildAvailable += async (client, e) => this.guild = e.Guild;
            client.VoiceStateUpdated += VoiceStateUpdated;
            client.MessageCreated += OnMessageCreated;
            Console.Error.WriteLine("[ENGINE] ... Handlers added");

            Console.Error.WriteLine("[ENGINE] Connecting ...");
            await client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] ... Connected");

            await WaitForGuildAsync();

            // cleanup
            await CleanupAsync();

            // create Channels
            await CreateDefaultChannelsAsync();

            Ready = true;
        }

        public async Task CleanupAsync()
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

        private async Task WaitForGuildAsync()
        {
            Console.Error.WriteLine("[ENGINE] waiting for guild ...");
            while (guild == null)
                await Task.Delay(500);
            Console.Error.WriteLine("[ENGINE] ... got guild");
        }

        #region Rooms Helpers
        public override async Task<TRoom> AddRoomAsync<TRoom>(TRoom room)
        {
            rooms.Add(room.Name, room);
            room.Game = this;
            room.VoiceChannel = await guild.CreateChannelAsync(room.Name, ChannelType.Voice, parentChannel);

            return room;
        }

        public IReadOnlyDictionary<string, Room> Rooms => rooms;
        #endregion


        #region Player Helpers
        public IReadOnlyDictionary<string, Player> Players => players.Values.ToDictionary(p => p.Name);

        private async Task<Player> GetOrCreatePlayer(DiscordMember member)
        {
            Player player;
            if (!players.TryGetValue(member.Id, out player))
            {
                Console.Error.WriteLine($"[ENGINE] Adding player {member.DisplayName} ...");
                player = new Player(member.DisplayName);
                players.Add(member.Id, player);

                player.Channel = await this.guild.CreateChannelAsync($"📜 {player.Name}", ChannelType.Text, parentChannel);

                player.User = member;

                player.Game = this;

                await player.InitAsync();

                Console.Error.WriteLine($"[ENGINE] ... Player {member.DisplayName} added");

            }
            return player;
        }
        #endregion
    }
}
