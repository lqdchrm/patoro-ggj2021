using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.LostAndFund

{
    public class CozyHut : BaseRoom
    {
        public bool FireTurnedOn = false;

        public override string Name => "Cozy Hut";

        [Command("FIRE", "Make a fire")]
        public async Task LightFire(Player player)
        {
            if (!FireTurnedOn)
            {
                FireTurnedOn = true;
                player.Room.SendGameEventAsync($"{player.Name} lightened a fire");
            } else
            {
                player.SendGameEventAsync("There is already a fire burning.");
            }
        }
    }
}
