using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class Fridge : Container
    {
        public Fridge(FindLostyGame game) : base(game)
        {
            Inventory.InitialAdd(new Tofu(game));
        }


        public override bool Use(Player sender, BaseThing<FindLostyGame, Room, Player, Thing> other, bool isFlippedCall)
        {
            var txt = UseText;

            sender.Reply(txt);
            return true;
        }
    }
}
