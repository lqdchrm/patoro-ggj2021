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
    public class Kitchen : CommonRoom
    {
        public override string Name => "Kitchen";
        #region LocalState
        bool FirePitOn = false;

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
                There are shelves at one wall and a large [refigerator] on the other.
                In the middle of the room is a large fire [pit]. {DescribeThing("pit", cmd)}
    {DescribeThing("roast",cmd)}
    There is one door leading to the [dinning room].
        ".FormatMultiline();
}

protected override string DescribeThing(string thing, GameCommand cmd)
{
    switch (thing.ToLowerInvariant())
    {

        case "pit":
            {
                if (FirePitOn)
                    return $"The fire is roaring.";
                else
                    return $"The fire is roaring.";
            }
        default:
            return base.DescribeThing(thing, cmd);
    }
}
        #endregion

        #region LISTEN
protected override string MakeSounds(GameCommand cmd)
{
    return "You hear barking from the back of the room.";
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

