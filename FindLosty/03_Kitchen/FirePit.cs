using LostAndFound.FindLosty._01_EntryHall;
using System.Linq;

namespace LostAndFound.FindLosty._03_Kitchen
{
    public class FirePit : Container
    {
        bool Burning = false;

        public FirePit(FindLostyGame game) : base(game, false, "FirePit")
        {
        }

        public override string LookText => this.Burning ? $"The fire is blazing." : $"The fire is out. The ash is still smoldering.";

        public override bool DoesItemFit(IThing thing, out string error)
        {
            error = $"You could 'put' things in there but this game won't let you... So maybe just 'use' them with the {this}.";
            return false;
        }

        public override void Use(IPlayer sender, IThing other)
        {
            if (other is null)
            {
                sender.Reply($"You feel really warm.");
            }
            else if (other is Splinters splinters)
            {
                BurnSplinters(sender, splinters);
            }
            else if (other is Tofu tofu)
            {
                BurnTofu(sender, tofu);
            }
        }

        public void BurnTofu(IPlayer sender, Tofu tofu)
        {
            if (Burning)
            {
                if  (tofu.Frozen)
                {
                    sender.Reply($"The tofu melts a little and the dripping water extinguished the fire.");
                    Burning = false;
                }
                else
                {
                    sender.Reply($"Your tofu is really warm now.");
                    tofu.Warm = true;
                }
            }
            else
            {
                sender.Reply($"The cold fire does absolutely nothing to your tofu.");
            }
        }

        public void BurnSplinters(IPlayer sender, Splinters splinters)
        {
            sender.Inventory.Transfer(splinters, Game.DiningRoom.Inventory);
            this.Burning = true;
            sender.Reply($"The smoldering ash is hot enough to make the splinters catch fire. The fire is burning again.");
        }
    }
}
