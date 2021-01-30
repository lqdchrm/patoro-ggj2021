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

        bool IsFireTurnedOn = false;
        bool KeysNoticed = false;

        protected override bool IsCommandVisible(string cmd)
        {
            switch (cmd)
            {
                case "FIRE": return true;
                case "DARK": return true;
                case "LOOK": return true;
            }
            return base.IsCommandVisible(cmd);
        }

        [Command("LIGHT", "Make a fire")]
        public async Task LightFire(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (!IsFireTurnedOn)
                {
                    IsFireTurnedOn = true;
                    await player.Room.SendGameEventAsync($"{player.Name} lightened a fire");
                }
                else
                {
                    await player.SendGameEventAsync("There is already a fire burning. You hurt yourself.");
                    await player.HitAsync("Fire");
                }
            }
        }

        [Command("DARK", "Extinguish fire")]
        public async Task ExtinguishFire(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (IsFireTurnedOn)
                {
                    IsFireTurnedOn = false;
                    await player.Room.SendGameEventAsync($"{player.Name} extinguished the fire");
                }
                else
                {
                    await player.SendGameEventAsync("The fire is already cold.");
                }
            }
        }

        [Command("LOOK", "Look around")]
        public async Task LookAround(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (IsFireTurnedOn)
                {
                    await player.SendGameEventAsync($"You look around and notice some keys on the floor.");
                    KeysNoticed = true;
                }
                else
                {
                    await player.SendGameEventAsync("It's too dark to see anything.");
                }
            }
        }

        [Command("PICKUP", "Pickup something")]
        public async Task Pickup(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                if (IsFireTurnedOn)
                {
                    var item = cmd.Args.FirstOrDefault();
                    if (item == null)
                        await player.SendGameEventAsync($"What do you want to pick up?");
                    else
                    {
                        if (item.ToLowerInvariant() == "keys" && KeysNoticed)
                        {
                            KeysNoticed = false;
                            player.Inventory.Add("keys", Emojis.Keys);
                            await player.SendGameEventWithStateAsync($"You picked up the keys.");
                        }
                    }
                    
                }
                else
                {
                    await player.SendGameEventAsync("It's too dark to see anything.");
                }
            }
        }
    }
}
