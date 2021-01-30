using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.Mansion
{
    public abstract class MansionRoom : Room<MansionGame, MansionPlayer, MansionRoom>
    {
        [Command("HELP", "Lists all available commands for this room")]
        public async Task HelpCommand(MansionPlayer player)
        {
            var intro = $"You are currently at {player.Room.Name}.\n";

            var commands = string.Join("\n", CommandDefs.Values
                .OrderBy(cmd => cmd.Name)
                .Select(cmd => $"{cmd.Name} - {cmd.Description}")
            );

            var msg = string.Join("\n", intro, commands);

            await player.SendMessage(msg);
        }


        //[Command("STATS", "Shows your stats")]
        //public async Task StatsCommand(MansionPlayer player) => await player.UpdateStatsAsync();


        //[Command("HEAL", "Increases your health")]
        //public async Task HealCommand(MansionPlayer player) => await player.HealAsync();

        //[Command("HIT", "Decreases your health")]
        //public async Task HitCommand(MansionPlayer player) => await player.HitAsync();
    }
}
