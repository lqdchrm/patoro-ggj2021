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

        #region LocalState
        bool FrontDoorHasBeenSeen = false;
        #endregion

        #region Inventory
        protected override IEnumerable<(string, string)> InitialInventory =>
            new List<(string, string)> {
                // ("keys", Emojis.Keys),
            };
        #endregion

        #region HELP
        protected override bool IsCommandVisible(string cmd)
        {
            switch(cmd.ToLowerInvariant())
            {
                case "open": return FrontDoorHasBeenSeen;
                case "knock": return FrontDoorHasBeenSeen;
                case "kick": return FrontDoorHasBeenSeen;

                default: return base.IsCommandVisible(cmd);
            }
        }
        #endregion

        #region LOOK
        protected override bool IsItemVisible(string itemKey)
        {
            return base.IsItemVisible(itemKey);
        }

        protected override string DescribeRoom(GameCommand cmd)
        {
            FrontDoorHasBeenSeen = true;

            string friends = string.Join(", ", Players.Where(p => p != cmd.Player).Select(p => $"[{p}]"));

            return $@"
                You're looking at the beautiful front yard of 404 Foundleroy Road.
                A picket fence surrounds the mansion in front of you.
                There seems to be only one way into the building. A large oak [door].
                This looks like some kind of maniac lives here.

                You hear barking.

                Your friends {friends} are here.
            ".FormatMultiline();
        }

        protected override string DescribeThing(string thing, GameCommand cmd) => thing switch
        {
            "door" => "A sturdy wooden door.",
            _ => null
        };
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return "You here distant barking. It definitely comes from the mansion in front of you.";
        }
        #endregion

        #region TAKE
        protected override string WhyIsItemNotTakeable(string itemKey)
        {
            return base.WhyIsItemNotTakeable(itemKey);
        }
        #endregion

        #region KICK
        protected override string KickThing(string thing, GameCommand cmd)
        {
            switch (thing.ToLowerInvariant())
            {
                case "door":
                    if (Game.State.FrontDoorOpen)
                        return "The open door hits the back wall and then swings back and hits your face.";
                    else
                    {
                        return "As the old saying goes: 'This will hurt you a lot more than it will the door.' The door shakes. You hurt.";
                    }
                default:
                    return $"You open [{thing}]. Throw it in the air and put it back.";
            }
        }
        #endregion

        #region OPEN
        protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        {
            switch(thing.ToLowerInvariant())
            {
                case "door":
                    if (Game.State.FrontDoorOpen)
                    {
                        return (true, "You open the door as much as possible.");
                    }
                    else
                    {
                        Game.State.FrontDoorOpen = true;
                        return (true, "The door swings open. Who doesn't lock their front door?");
                    }
                default:
                    return (false, "Open what?");
            }
        }
        #endregion

        #region Custom commands: KNOCK

        [Command("KNOCK", "knock on something")]
        public void KnockCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;
            string message = "I SEE ERROR";

            if (cmd.Args.Count == 0)
            {
                message = "Knock what?";
            }
            else if (cmd.Args.Count == 1 && cmd.Args[0] == "door")
            {
                if (Game.State.FrontDoorOpen)
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

             player.SendGameEvent(message);
        }
        #endregion
    }
}
