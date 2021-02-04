using LostAndFound.FindLosty._01_EntryHall;
using System.Linq;

namespace LostAndFound.FindLosty._03_Kitchen
{
    public class FirePit : Container
    {
        bool Burning = false;

        public FirePit(FindLostyGame game) : base(game, false, "pit")
        {
        }

        public override string LookText
        {
            get
            {
                if (this.Burning)
                    return $"The fire is blazing.";
                else
                    return $"The fire is out. The ash is still smoldering.";
            }
        }

        public override bool DoesItemFit(IThing thing, out string error)
        {
            IThing maybeTofu = this.Inventory.FirstOrDefault();
            if (maybeTofu == null)
            {
                return base.DoesItemFit(thing, out error);
            }
            else
            {
                error = $"There only is room for {maybeTofu}.";
                return false;
            }
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
            else
            {
                if (this.Burning)
                {
                    sender.Reply($"Your {other.Name} is now really warm.");
                }
                else
                {
                    sender.Reply($"The fire is not warm enough to do anything.");
                }
            }
        }

        private void BurnSplinters(IPlayer sender, Splinters splinters)
        {
            sender.Inventory.Remove(splinters);
            this.Burning = true;
            sender.Reply($"The smoldering ash is hot enough to make the splinters catch fire. The fire is burning again.");
        }
    }
}
