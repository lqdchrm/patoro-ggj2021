using DSharpPlus.Entities;
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
    public class TobisWorld : CommonRoom
    {
        public bool FireTurnedOn = false;

        public override string Name => "Tobis World";

        [Command("look", "look around")]
        public async Task Look(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (!FireTurnedOn)
                {
                    FireTurnedOn = true;
                    await player.Room.SendGameEventAsync($"{player.Name} lightened a fire");
                }
                else
                {
                    await player.SendGameEventAsync("There is already a fire burning.");
                }
            }
        }
    }
}
