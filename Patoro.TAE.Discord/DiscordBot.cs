using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patoro.TAE.Discord
{
    public class DiscordBot
    {
        private DiscordClient client;
        private string DefaultGame;

        private const Permissions neededPermissions =
                  Permissions.DeafenMembers
                | Permissions.ManageChannels
                | Permissions.ManageRoles
                | Permissions.MoveMembers
                | Permissions.AccessChannels
                | Permissions.MuteMembers
                | Permissions.SendMessages
                | Permissions.SendTtsMessages;

        private readonly DiscordGameRegistry GameRegistry = new DiscordGameRegistry();
        private readonly Dictionary<ulong, DiscordGuildBotInstance> BotLoockup = new Dictionary<ulong, DiscordGuildBotInstance>();

        public void RegisterGame(string name, Func<string, DiscordClient, DiscordGuild, IGame> gameCtor)
        {
            GameRegistry.Add(name, gameCtor);
        }

        public async Task Start(string defaultGame = null)
        {
            this.DefaultGame = defaultGame;
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

            Console.WriteLine("Use following URL to invite the bot to your Server:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"https://discord.com/api/oauth2/authorize?client_id={client.CurrentUser.Id}&permissions={(long)neededPermissions}&scope=bot");
            Console.ResetColor();

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

            // check permissions
            if (e.Guild.Permissions is not Permissions permissions)
            {
                Console.Error.WriteLine($"[ENGINE WARNING] Could not check permissions on Guild {e.Guild.Name}");
            }
            else if (!permissions.HasPermission(neededPermissions))
            {
                Console.Error.WriteLine($"[ENGINE ERROR] Guild {e.Guild.Name} does not have enough permissions");
                var missingPermissions = Enum.GetValues<Permissions>()
                    .Where(p => neededPermissions.HasFlag(p))
                    .Where(p => !permissions.HasPermission(p));
                Console.Error.WriteLine($"[ENGINE ERROR] Guild {e.Guild.Name} need {string.Join(", ", missingPermissions)}");
            }

            var server = new DiscordGuildBotInstance(sender, e.Guild, GameRegistry, DefaultGame);
            this.BotLoockup.Add(e.Guild.Id, server);
            return Task.CompletedTask;
        }
    }
}
