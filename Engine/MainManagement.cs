using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

    class ServerBot : IDisposable
    {
        private DiscordClient client;
        private DiscordGuild guild;
        private bool disposedValue;

        private readonly Regex gameRegex = new Regex(@"(?<game>\w+)\s+(?<instance>.+)\s*$");

        private Dictionary<string, Func<string, DiscordClient, DiscordGuild, DiscordGame>> GameMapping = new Dictionary<string, Func<string, DiscordClient, DiscordGuild, DiscordGame>>()
        {
            {"LostAndFound", (instanceName, client, guild) => new Game.LostAndFoundGame(instanceName,client,guild) },
            {"Mansion", (instanceName, client, guild) => new Game.LostAndFoundGame(instanceName,client,guild) }
        };

        private readonly Dictionary<string, DiscordGame> gameLookup = new Dictionary<string, DiscordGame>();

        public ServerBot(DiscordClient sender, DiscordGuild guild)
        {
            this.client = sender;
            this.guild = guild;
            client.MessageCreated += this.Client_MessageCreated;
        }


        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Guild.Id != this.guild.Id)
                return Task.CompletedTask;

            var match = gameRegex.Match(e.Message.Content);


            if (e.MentionedUsers.Any(x => x.Id == client.CurrentUser.Id))
            {



                var gameName = match.Groups["game"].Value;
                var instanceName = match.Groups["instance"].Value;


                var game = GameMapping.FirstOrDefault(entry => entry.Key == gameName).Value?.Invoke(instanceName, client, guild);

                if (match.Success && game is not null)
                {
                    if (gameLookup.TryGetValue(instanceName, out var oldGame))
                        oldGame.Dispose();
                    gameLookup[instanceName] = game;
                    game.StartAsync();
                }
                else
                {

                    e.Message.RespondAsync(@$"Not a valid game. Valid Games are
 - {string.Join("\n - ", GameMapping.Keys)}

Use `[GameName] [InstanceName]` to create a game");
                }
                e.Handled = true;
            }
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
                if (disposing)
                {
                    client.MessageCreated -= this.Client_MessageCreated;

                }

            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
