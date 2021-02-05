﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Discord
{
    internal class DiscordGuildBotInstance : IDisposable
    {
        private DiscordClient client;
        private DiscordGuild guild;
        private bool disposedValue;

        private readonly Regex gameRegex = new Regex(@"(?<game>\w+)\s+(?<instance>.+)\s*$");

        private Dictionary<string, Func<string, DiscordClient, DiscordGuild, IGame>> GameMapping = new Dictionary<string, Func<string, DiscordClient, DiscordGuild, IGame>>()
        {
            //{"LostAndFound", (instanceName, client, guild) => new Game.LostAndFound.LostAndFoundGame(instanceName,client,guild) },
            //{"Mansion", (instanceName, client, guild) => new Game.Mansion.MansionGame(instanceName,client,guild) },
            {"FindLosty", (instanceName, client, guild) => FindLosty.FindLostyGame.Discord(instanceName, client, guild) }
        };

        private readonly Dictionary<string, IGame> gameLookup = new Dictionary<string, IGame>();

        public DiscordGuildBotInstance(DiscordClient sender, DiscordGuild guild, Type defaultGame = null)
        {
            this.client = sender;
            this.guild = guild;

            this.client.MessageCreated += this.Client_MessageCreated;

            // TODO: Find the default text channel without using hardcoded strings...
            var defaultChanel =
                guild.Channels.FirstOrDefault(x => x.Value.Name == "Allgemein" && x.Value.Type == ChannelType.Text).Value
                ?? guild.Channels.FirstOrDefault(x => x.Value.Type == ChannelType.Text).Value;

            if (defaultGame != null)
            {
                var gameName = defaultGame.Name.Replace("Game", "");
                var game = this.GameMapping.FirstOrDefault(entry => entry.Key == gameName).Value?.Invoke(gameName, this.client, guild);
                if (game != null)
                {
                    this.gameLookup[gameName] = game;
                    _ = game.StartAsync();
                }

            }
            else
            {
                if (defaultChanel is not null)
                    Welcome(defaultChanel);
            }
        }

        private async void Welcome(DiscordChannel defaultChanel)
        {
            await defaultChanel.SendMessageAsync("Hi I'm your happy GM. You can Mention me if you want to start a game");
            await defaultChanel.SendMessageAsync(GetValidGames());
        }

        private Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Guild.Id != this.guild.Id)
                return Task.CompletedTask;

            var match = this.gameRegex.Match(e.Message.Content);

            if (e.MentionedUsers.Any(x => x.Id == this.client.CurrentUser.Id))
            {

                var gameName = match.Groups["game"].Value;
                var instanceName = match.Groups["instance"].Value;

                var game = this.GameMapping.FirstOrDefault(entry => entry.Key == gameName).Value?.Invoke(instanceName, this.client, this.guild);

                if (match.Success && game is not null)
                {
                    if (this.gameLookup.TryGetValue(instanceName, out var oldGame))
                        oldGame.Dispose();
                    this.gameLookup[instanceName] = game;
                    _ = game.StartAsync();
                }
                else
                {
                    e.Message.RespondAsync(@$"Not a valid game.");
                    e.Message.RespondAsync(GetValidGames());
                }
                e.Handled = true;
            }
            return Task.CompletedTask;
        }

        private string GetValidGames()
        {
            return @$"Valid Games are
 - {string.Join("\n - ", this.GameMapping.Keys)}

Use `[GameName] [InstanceName]` to create a game";
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
