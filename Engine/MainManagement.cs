using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    class MainManagement
    {
        private DiscordClient client;
        private readonly Dictionary<ulong, ServerBot> BotLoockup = new Dictionary<ulong, ServerBot>();

        public async Task Start()
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
            client.GuildAvailable += ServerActive;
            client.GuildCreated += ServerActive;
            client.GuildDeleted += ServerDeactivated;

            Console.Error.WriteLine("[ENGINE] ... Handlers added");

            Console.Error.WriteLine("[ENGINE] Connecting ...");
            await client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] ... Connected");
        }

        private async Task ServerDeactivated(DiscordClient sender, GuildDeleteEventArgs e)
        {
            if (BotLoockup.TryGetValue(e.Guild.Id, out var bot))
            {
                bot.Dispose();
                BotLoockup.Remove(e.Guild.Id);
            }
        }

        private async Task ServerActive(DiscordClient sender, GuildCreateEventArgs e)
        {
            Console.Error.WriteLine($"[ENGINE] ... Guild added {e.Guild.Name}");

            var server = new ServerBot(sender, e.Guild);
            BotLoockup.Add(e.Guild.Id, server);
        }
    }
}
