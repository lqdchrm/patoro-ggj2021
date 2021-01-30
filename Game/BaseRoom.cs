using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game
{
    public abstract class BaseRoom : Room
    {
        [Command("HELP", "Lists all available commands for this room")]
        public async Task HelpCommand(Player player)
        {
            var intro = $"You are currently at {player.Room.Name}.\n";

            var commands = string.Join("\n", CommandDefs.Values
                .OrderBy(cmd => cmd.Name)
                .Select(cmd => $"{cmd.Name} - {cmd.Description}")
            );

            var msg = string.Join("\n", intro, commands);

            await player.SendGameEventAsync(msg);
        }


        [Command("STATS", "Shows your stats")]
        public async Task StatsCommand(Player player) => await player.UpdateStatsAsync();


        [Command("HEAL", "Increases your health")]
        public async Task HealCommand(Player player) => await player.HealAsync();

        [Command("HIT", "Decreases your health")]
        public async Task HitCommand(Player player) => await player.HitAsync();
    }
}
