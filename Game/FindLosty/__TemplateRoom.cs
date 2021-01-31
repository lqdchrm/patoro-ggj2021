using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class TemplateRoom : CommonRoom
    {
        public override string Name => throw new NotImplementedException();

        #region LocalState
        #endregion

        #region Inventory
        protected override IEnumerable<(string, string, string)> InitialInventory =>
            new List<(string, string, string)> {
                // ("keys", Emojis.Keys, "Some keys"),
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
            return base.DescribeRoom(cmd);
        }

        protected override string DescribeThing(string thing, GameCommand cmd)
        {
            return base.DescribeThing(thing, cmd);
        }
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return base.MakeSounds(cmd);
        }
        protected override string ListenAtThing(string thing, GameCommand cmd)
        {
            return base.ListenAtThing(thing, cmd);
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
