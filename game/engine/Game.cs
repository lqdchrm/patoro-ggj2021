using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using DSharpPlus.Net;
using System.Linq;

namespace LostAndFound.Engine
{
    public abstract class Game
    {
        protected DiscordGuild guild;
        protected DiscordChannel parentChannel;
        protected readonly Dictionary<string, DiscordChannel> voiceChannels = new Dictionary<string, DiscordChannel>();
        protected readonly Dictionary<ulong, DiscordChannel> userChannels = new Dictionary<ulong, DiscordChannel>();
        protected DiscordChannel helpChannel;

        public async Task Start()
        {
            DotNetEnv.Env.Load();

            var client = new DiscordClient(new DiscordConfiguration
            {
                Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot
            });

            Console.Error.WriteLine("[ENGINE] Discord client created...");

            client.MessageCreated += OnMessageCreated;
            Console.Error.WriteLine("[ENGINE] Handlers added...");

            client.GuildAvailable += OnGuildAvailable;
            client.VoiceStateUpdated += VoiceStateUpdated;

            await client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] Connected...");

            await Task.Delay(-1);
        }

        protected abstract Task InitGame();

        protected async Task<Room> CreateRoomAsync(string name)
        {
            var room = await guild.CreateChannelAsync(name, ChannelType.Voice, parentChannel);
            voiceChannels.Add(name, room);
            return new Room(room);
        }

        private async Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs e)
        {
            try
            {
                // cleanup
                var oldGroup = e.Guild.Channels.Values.FirstOrDefault(c => c.Name == "LostAndFoundGame");
                if (oldGroup != null)
                {
                    foreach (var child in oldGroup.Children)
                    {
                        await child.DeleteAsync();
                    }
                    await oldGroup.DeleteAsync();
                }
                this.guild = e.Guild;

                // create Channels
                parentChannel = await this.guild.CreateChannelAsync("LostAndFoundGame", ChannelType.Category);

                helpChannel = await this.guild.CreateChannelAsync("GameHelp", ChannelType.Text, parentChannel);

                InitGame();
            }
            catch (Exception exc)
            {

            }
        }

        private async Task VoiceStateUpdated(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            var oldChannel = e.Before?.Channel;
            var newChannel = e.After?.Channel;

            if (oldChannel == newChannel)
                return;

            if (oldChannel != null && voiceChannels.ContainsKey(oldChannel.Name))
            {
                if (e.User is DiscordMember member)
                {
                    var channel = await GetUserChannelAsync(member);
                    await channel.SendMessageAsync($"[Gamemaster]: You left {oldChannel.Name}");

                    await BroadcastToUserRoomAsync(member, "Gamemaster", $"{member.DisplayName} left {oldChannel.Name}");
                }
            }

            if (newChannel != null && voiceChannels.ContainsKey(newChannel.Name))
            {
                if (e.User is DiscordMember member)
                {
                    var channel = await GetUserChannelAsync(member);
                    await channel.SendMessageAsync($"[Gamemaster]: You entered {newChannel.Name}");

                    await BroadcastToUserRoomAsync(member, "Gamemaster", $"{member.DisplayName} entered {newChannel.Name}");
                }
            }
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            Console.Error.WriteLine($"[MESSAGE] received: {e.Message.Content}");

            if (e.Author != client.CurrentUser)
            {
                if (e.Message.Content.StartsWith("!"))
                {
                    // handle commands
                    switch(e.Message.Content)
                    {
                        case "!help": await helpChannel.SendMessageAsync("It's dangerous to go alone. Take these ⚔"); break;
                    }
                }
                else
                {
                    // broadcast to room
                    if (e.Author is DiscordMember member)
                    {
                        await BroadcastToUserRoomAsync(member, member.DisplayName, e.Message.Content);
                    }
                }
            }
        }

        private async Task<DiscordChannel> GetUserChannelAsync(DiscordMember user)
        {
            DiscordChannel userChannel;
            if (!userChannels.TryGetValue(user.Id, out userChannel))
            {
                userChannel = await user.Guild.CreateChannelAsync(user.DisplayName, ChannelType.Text, parentChannel);
                userChannels.Add(user.Id, userChannel);
            }
            return userChannel;
        }

        private async Task BroadcastToUserRoomAsync(DiscordMember user, string from, string msg)
        {
            var room = voiceChannels.Values.FirstOrDefault(channel => channel.Users.Contains(user));
            if (room != null)
            {
                foreach (var other in room.Users.Where(u => u != user))
                {
                    var channel = await GetUserChannelAsync(other);
                    await channel.SendMessageAsync($"[{from}]: {msg}");
                }
            }
        }
    }
}
