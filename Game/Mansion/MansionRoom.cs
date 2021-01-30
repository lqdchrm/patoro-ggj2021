using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.Mansion
{
    public abstract class MansionRoom : BaseRoom
    {
        protected override bool IsCommandVisible(string cmdName) => true;

        [Command("HELP", "Lists all available commands for this room")]
        public async Task HelpCommand(PlayerCommand cmd)
        {
            if (cmd.Player is MansionPlayer player)
            {
                var intro = $"You are currently at {player.Room.Name}.\n";

                var commands = string.Join("\n", Commands
                    .OrderBy(cmd => cmd.Name)
                    .Select(cmd => $"{cmd.Name} - {cmd.Description}")
                );

                var msg = string.Join("\n", intro, commands);

                await player.SendMessage(msg);
            }
        }
    }
}
