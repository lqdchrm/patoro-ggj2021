using DSharpPlus;
using DSharpPlus.EventArgs;
using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Discord
{
    class DiscordBot
    {
        private Type defaultGame;
        private DiscordClient client;
        private readonly Dictionary<ulong, DiscordGuildBotInstance> BotLoockup = new Dictionary<ulong, DiscordGuildBotInstance>();

        public async Task Start(Type defaultGame = null)
        {
            this.defaultGame = defaultGame;

            DotNetEnv.Env.Load();

            Console.Error.WriteLine("[ENGINE] Creating Discord client ...");
            this.client = new DiscordClient(new DiscordConfiguration
            {
                Token = System.Environment.GetEnvironmentVariable("DISCORD_TOKEN"),
                TokenType = TokenType.Bot,
            });
            Console.Error.WriteLine("[ENGINE] ... Discord client created");

            Console.Error.WriteLine("[ENGINE] Adding Handlers ...");
            this.client.GuildAvailable += GuildActive;
            this.client.GuildCreated += GuildActive;
            this.client.GuildDeleted += GuildDeactivated;
            Console.Error.WriteLine("[ENGINE] ... Handlers added");

            Console.Error.WriteLine("[ENGINE] Connecting ...");
            await this.client.ConnectAsync();
            Console.Error.WriteLine("[ENGINE] ... Connected");
        }

        private Task GuildDeactivated(DiscordClient sender, GuildDeleteEventArgs e)
        {
            if (this.BotLoockup.TryGetValue(e.Guild.Id, out var bot))
            {
                bot.Dispose();
                this.BotLoockup.Remove(e.Guild.Id);
            }
            return Task.CompletedTask;
        }

        private Task GuildActive(DiscordClient sender, GuildCreateEventArgs e)
        {
            Console.Error.WriteLine($"[ENGINE] ... Guild added {e.Guild.Name}");

            var server = new DiscordGuildBotInstance(sender, e.Guild, this.defaultGame);
            this.BotLoockup.Add(e.Guild.Id, server);
            return Task.CompletedTask;
        }
    }
}
