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

        public override bool DoesItemFit(BaseThing<FindLostyGame, Room, Player, Thing> thing, out string error)
        {
            BaseThing<FindLostyGame, Room, Player, Thing> item = null;
            bool found_tofu = Inventory.TryFind("tofu", out item);
            if (found_tofu)
            {
                error = $"You can't put that in. there is {item} inside the microwave.";
                return false;
            }
            else if (thing is Tofu tofu)
            {
                error = "";
                return true;
            }
            else
            {
                error = $"I don't really feel like putting {item} into the microwave.";
                return false;
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
