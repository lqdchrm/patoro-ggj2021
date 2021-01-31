using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class FrontYard : CommonRoom
    {

        public override string Name => "FrontYard";

        public FrontYard() : base()
        {
            Inventory.Add("keys", Emojis.Keys);
        }

        protected override string WhyIsItemNotTakeable(string itemKey)
        {
            return itemKey switch
            {
                "keys" => "better not",
                    _ => null
                    };
        }

        protected override bool IsCommandVisible(string cmd)
        {
            switch (cmd)
            {
            }
            return base.IsCommandVisible(cmd);
        }

        public override async Task Listen(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "You here distant barking. It definitely comes from the mansion in front of you.";

            await player.SendGameEventAsync(message);
        }

        public override async Task LookAt(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "I SEE ERROR";

            if (cmd.Args.Count == 0) {

                string friends = "";
                var player_list = Game.Players.Values.Where(p => p.Room == this);
                foreach (Player p in player_list)
                {
                    friends += p.Name + " ";
                }
                message = $@"You're looking at the beautiful front yard of 404 Foundleroy Road.
A picket fence surrounds the mansion in front of you.
There seems to be only one way into the building. A large oak door.
This looks like some kind of maniac lives here.

You hear barking.

Your friends {friends} are here.
                ";
            }
            else if (cmd.Args.Count == 1 && cmd.Args[0] == "door")
            {
                message = "A sturdy wooden door.";
            }
            else
            {
                message = $"You can't see a {cmd.Args[0]}";
            }
            await player.SendGameEventAsync(message);
        }

        [Command("KICK", "kick something")]
        public async Task KickCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "I SEE ERROR";

            if (cmd.Args.Count == 0)
            {
                message = "Kick what?";
            }
            else if (cmd.Args.Count == 1 && cmd.Args[0] == "door")
            {
                if (GameState.FrontDoorOpen)
                {
                    message = "The open door hits the back wall and then swings back and hits your face.";
                } else
                {
                    message = "As the old saying goes: 'This will hurt you a lot more than it will the door.' The door shakes. You hurt.";
                }
            }
            else
            {
                message = $"You open {cmd.Args[0]}. Throw it in the air and put it back.";
            }

            await player.SendGameEventAsync(message);
        }

        [Command("OPEN", "open something")]
        public async Task KnockCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "I SEE ERROR";

            if (cmd.Args.Count == 0)
            {
                message = "Open what?";
            }
            else if (cmd.Args.Count == 1 && cmd.Args[0] == "door")
            {
                if (GameState.FrontDoorOpen)
                {
                    message = "You open the door as much as possible.";
                } else
                {
                    GameState.FrontDoorOpen = true;
                    message = "The door swings open. Who doesn't lock their front door?";
                }
            }
            else
            {
                message = $"You open {cmd.Args[0]}. Throw it in the air and put it back.";
            }

            await player.SendGameEventAsync(message);
        }

        [Command("KNOCK", "knock on something")]
        public async Task OpenCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "I SEE ERROR";

            if (cmd.Args.Count == 0)
            {
                message = "Knock what?";
            }
            else if (cmd.Args.Count == 1 && cmd.Args[0] == "door")
            {
                if (GameState.FrontDoorOpen)
                {
                    message = "You knock on an open door. Still no one answers.";
                } else
                {
                    message = "You knock on the door. No one answers.";
                }
            }
            else
            {
                message = $"You knock {cmd.Args[0]} really hard.... Nothing happens.";
            }

            await player.SendGameEventAsync(message);
        }
    }
}
