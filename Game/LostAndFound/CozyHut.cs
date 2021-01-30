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
    public class CozyHut : CommonRoom
    {
        public override string Name => "Cozy Hut";

        #region STATE

        public bool FireTurnedOn = false;

        #endregion

        protected override bool IsCommandVisible(string cmd)
        {
            //switch(cmd)
            //{
            //    case "FIRE": return false;
            //}
            return base.IsCommandVisible(cmd);
        }

        [Command("FIRE", "Make a fire")]
        public async Task LightFire(PlayerCommand cmd)
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
                    await player.SendGameEventAsync("There is already a fire burning. You hurt yourself.");
                    await player.HitAsync("Fire");
                }
            }
        }
    }
}
