using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class Microwave : Container
    {

        public Microwave(FindLostyGame game) : base(game)
        {
        }


        public override string UseText
        {
            get { return$"This is the best fridge ever."; }
        }

        public override void Put(Player sender, BaseThing<FindLostyGame, Room, Player, Thing> other)
        {
            BaseThing<FindLostyGame, Room, Player, Thing> item = null;
            bool found_tofu = Inventory.TryFind("tofu", out item);
            if (found_tofu)
            {
                sender.Reply("You can't put that in. there is {item} inside the microwave.");
            }
            else if (other is Tofu tofu)
            {
                sender.Inventory.Transfer("tofu", Inventory);
                sender.Reply("The {tofu} is inside the microwave now.");
            }
            else
            {
                sender.Reply("I don't really feel like putting {item} into the microwave.");
            }
        }

        public override bool Use(Player sender, BaseThing<FindLostyGame, Room, Player, Thing> other, bool isFlippedCall)
        {
            BaseThing<FindLostyGame, Room, Player, Thing> item = null;
            bool found_tofu = Inventory.TryFind("tofu", out item);
            if (found_tofu && item is Tofu tofu)
            {
                tofu.Frozen = false;
                sender.Reply("The tofu immediately unfreezes.");
            }
            else
            {
                sender.Reply("The microwave turns on for 30 seconds. Nothing really happens. You wasted some energy. Somewhere in the rain forest a tree dies.");
            }
            return true;
        }
    }
}
