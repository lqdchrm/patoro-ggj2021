using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.LostAndFound
{
    public abstract class CommonRoom : BaseRoom
    {
        protected override bool IsCommandVisible(string cmd) => true;

        [Command("HELP", "Lists all available commands for this room")]
        public async Task HelpCommand(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                var intro = $"You are currently at {player.Room.Name}.\n";

                var commands = string.Join("\n", Commands
                    .OrderBy(cmd => cmd.Name)
                    .Select(cmd => $"{cmd.Name} - {cmd.Description}")
                );

                var msg = string.Join("\n", intro, commands);

                await player.SendGameEventAsync(msg);
            }
        }

        [Command("HEAL", "Increases your health")]
        public async Task HealCommand(PlayerCommand cmd) => await (cmd.Player as Player)?.HealAsync();

        [Command("HIT", "Decreases your health")]
        public async Task HitCommand(PlayerCommand cmd)
        {
            var tasks = cmd.Mentions.Cast<Player>().Select(p => p.HitAsync());
            await Task.WhenAll(tasks);
        }
    }
}
