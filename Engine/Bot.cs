using DSharpPlus;
using DSharpPlus.EventArgs;
using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound
{
    class Bot
    {
        private Type defaultGame;
        private DiscordClient client;
        private readonly Dictionary<ulong, GuildBotInstance> BotLoockup = new Dictionary<ulong, GuildBotInstance>();

        public async Task Start(Type defaultGame = null)
        {
            this.defaultGame = defaultGame;

            DotNetEnv.Env.Load();

            Console.Error.WriteLine("[ENGINE] Creating Discord client ...");
            client = new DiscordClient(new DiscordConfiguration
            {
                Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
            });
            Console.Error.WriteLine("[ENGINE] ... Discord client created");

            Console.Error.WriteLine("[ENGINE] Adding Handlers ...");
            client.GuildAvailable += GuildActive;
            client.GuildCreated += GuildActive;
            client.GuildDeleted += GuildDeactivated;
            Console.Error.WriteLine("[ENGINE] ... Handlers added");

            Console.Error.WriteLine("[ENGINE] Connecting ...");
            await client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] ... Connected");
        }

        private async Task GuildDeactivated(DiscordClient sender, GuildDeleteEventArgs e)
        {
            if (BotLoockup.TryGetValue(e.Guild.Id, out var bot))
            {
                bot.Dispose();
                BotLoockup.Remove(e.Guild.Id);
            }
        }

        private async Task GuildActive(DiscordClient sender, GuildCreateEventArgs e)
        {
            Console.Error.WriteLine($"[ENGINE] ... Guild added {e.Guild.Name}");

            var server = new GuildBotInstance(sender, e.Guild, defaultGame);
            BotLoockup.Add(e.Guild.Id, server);
        }
    }
}
