using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class Fridge : Thing
    {
        public Fridge(FindLostyGame game) : base(game)
        {
        }


        public override string UseText
        {
            get { return$"This is the best fridge ever"; }
        }

        public override bool Use(Player sender, BaseThing<FindLostyGame, Room, Player, Thing> other, bool isFlippedCall)
        {
            var txt = UseText;

            sender.Reply(txt);
            return true;
        }
    }
}
