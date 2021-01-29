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
    public class Game
    {
        protected DiscordClient client;
        
        protected DiscordChannel help;
        protected DiscordChannel taverne;
        protected DiscordChannel taverneText;

        public async Task Start()
        {
            DotNetEnv.Env.Load();

            this.client = new DiscordClient(new DiscordConfiguration
            {
                Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot
            });

            Console.Error.WriteLine("[ENGINE] Discord client created...");

            client.MessageCreated += OnMessageCreated;
            Console.Error.WriteLine("[ENGINE] Handlers added...");

            client.GuildAvailable += OnGuildAvailable;

            await client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] Connected...");

            await Task.Delay(-1);
        }

        private async Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs e)
        {
            try
            {
                var oldGroup = e.Guild.Channels.Values.First(c => c.Name == "LostAndFoundGame");
                foreach (var child in oldGroup.Children)
                {
                    await child.DeleteAsync();
                }
                await oldGroup.DeleteAsync();

                // cleanup
                var parent = await e.Guild.CreateChannelAsync("LostAndFoundGame", ChannelType.Category);

                help = await e.Guild.CreateChannelAsync("GameHelp", ChannelType.Text, parent);
                taverne = await e.Guild.CreateChannelAsync("Taverne", ChannelType.Voice, parent);
                taverneText = await e.Guild.CreateChannelAsync("TaverneText", ChannelType.Text, parent);

            }
            catch (Exception exc)
            {

            }
        }

        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            Console.Error.WriteLine($"[MESSAGE] received: {e.Message.Content}");

            if (e.Author != client.CurrentUser)
            {
                switch (e.Message.Content)
                {
                    case "help": await help.SendMessageAsync("It's dangerous to go alone. Take these ⚔"); break;
                }
            }
        }
    }
}
