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
    public class EntryHall : CommonRoom
    {
        public override string Name => "EntryHall";

        #region LocalState
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
            return base.IsCommandVisible(cmd);
        }
        #endregion

        #region LOOK
        protected override bool IsItemVisible(string itemKey)
        {
            return base.IsItemVisible(itemKey);
        }

        protected override string DescribeRoom(GameCommand cmd)
        {
            return $@"
                You are in a great hall. The floor has a black and white checker pattern.
                To your left is the [wardrobe] and to your right a small table with a [phone].

                In the middle of the hall, on both sides in the wall, you'll find two doors.

                Both the [left door] and the [right door] are made of a massive dark wood.

                In the back of the hall is a wide [staircase]. You can also see a [window] on the back of the room if you look past the [staircase].

                The barking gets loader.
            ".FormatMultiline();
        }

        protected override string DescribeThing(string thing, GameCommand cmd)
        {
            switch (thing.ToLowerInvariant())
            {

                case "wardrobe":
                    return $"The [wardrobe] can hold many coats and cloaks.But it is empty.";
                
                case "phone":
                    return $"An old dark [phone] with an dialplate. The decorative numbers are written on a white circle.\nThe 6 looks very used.";
                
                case "left-door":
                    return $"The massive door made of dark wood must have been once very beautiful. You see some water marks on the side where the door has swollen up a little.";
                
                case "right-door":
                    return $"The massive door made of dark wood is still in good condition.";
                
                case "staircase":
                    return @$"Like many elements in this room the stairs are made of a dark wood.
                              But it looks old and ... not in a good way. It could be a hazard.
                              In the side of the staircase is a [metal door].".FormatMultiline();

                case "window":
                    return $"You look into the garden at the back of the house. It could need some care...";

                case "metal-door":
                    return @$"A very study door. It would be blast to open it.";
                
                default:
                    return base.DescribeThing(thing, cmd);
            }
        }
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return "You hear a barking from the back of the room.";
        }

        protected override string ListenAtThing(string thing, GameCommand cmd)
        {
            switch (thing)
            {
                case "metal-door": return $"You hear scratching on the other side of the very massive door.";
                case "phone": return $"Beeeeeeeeeeeeeeeeeeeeeeeeeeeeeep.....";
                default: return base.ListenAtThing(thing, cmd);
            }
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
            return base.KickThing(thing, cmd);
        }
        #endregion


        #region OPEN
        protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        {
            return base.OpenThing(thing, cmd);
        }
        #endregion
    }
}
