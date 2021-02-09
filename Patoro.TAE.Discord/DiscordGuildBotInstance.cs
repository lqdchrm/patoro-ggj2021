using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Patoro.TAE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Patoro.TAE.Discord
{
    internal class DiscordGuildBotInstance : IDisposable
    {
        private DiscordClient client;
        private DiscordGuild guild;
        private DiscordGameRegistry registry;

        private bool disposedValue;

        private readonly Dictionary<string, IGame> gameLookup = new Dictionary<string, IGame>();

        public DiscordGuildBotInstance(DiscordClient sender, DiscordGuild guild, DiscordGameRegistry registry, string defaultGame = null)
        {
            this.client = sender;
            this.guild = guild;
            this.registry = registry;

            this.client.MessageCreated += this.Client_MessageCreated;

            if (defaultGame != null)
            {
                StartGame(defaultGame);
            }
            else
            {
                _ = Welcome();
            }
        }

        public async Task Welcome()
        {
            var games = await GamesList();
            var msg = @$"```css
                Hi I'm your happy 🤖GameMaster. Tell me when you want to start a game.

                {games}
                ```".FormatMultiline();

            guild.GetDefaultChannel()?.SendMessageAsync(msg);
        }

        public async Task<string> GamesList()
        {
            var member = await this.guild.GetMemberAsync(this.client.CurrentUser.Id);

            return $@"
                You can currently select the following games:
                - {string.Join("\n - ", registry.Keys)}

                Tell me which one you want to start like so:
                > @{member.DisplayName} start [GameName]
                ".FormatMultiline();
        }

        private bool StartGame(string gameName, string instanceName = null)
        {
            instanceName = instanceName ?? gameName;

            IGame game = null;
            if (registry.TryGetValue(gameName, out Func<string, DiscordClient, DiscordGuild, IGame> gameCtor))
                game = gameCtor.Invoke(instanceName, this.client, this.guild);

            if (game is not null)
            {
                // cleanup old game
                if (this.gameLookup.TryGetValue(instanceName, out var oldGame)) oldGame.Dispose();

                // start
                this.gameLookup[instanceName] = game;
                _ = game.StartAsync();
                return true;
            }
            return false;
        }

        private async Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            // Check Guild
            if (e.Guild.Id != this.guild.Id) return;

            // Check Channel
            if (e.Channel != this.guild.GetDefaultChannel()) return;

            // Check Bot Mention
            if (!e.MentionedUsers.Any(x => x.Id == this.client.CurrentUser.Id)) return;

            // Check Valid Start Command
            var gameRegex = new Regex(@"start\s+(?<game>\w+)$");
            var match = gameRegex.Match(e.Message.Content);
            var gameName = match.Groups["game"].Value;

            // TODO: re-implement instanceName logic
            var instanceName = gameName;

            if (!StartGame(gameName, instanceName))
            {
                var games = await GamesList();

                var msg = $@"```css
                    I don't know this game.
                    {games}
                    ```".FormatMultiline();

                await e.Message.RespondAsync(msg);
            }
            e.Handled = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                this.disposedValue = true;
                if (disposing)
                {
                    this.client.MessageCreated -= this.Client_MessageCreated;

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
