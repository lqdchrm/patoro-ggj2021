using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class FirePit : Container
    {
        bool Burning = false;
        
        public FirePit(FindLostyGame game) : base(game, false, "pit")
        {
        }

        public override string LookText
        {
            get {
                if (Burning)
                    return $"The fire is blazing.";
                else
                    return $"The fire is out. The ash is still smoldering.";
            }
        }
        
        public override bool DoesItemFit(IThing thing, out string error)
        {
            error = "";
            IThing maybeTofu = Inventory.FirstOrDefault();
            if (maybeTofu == null)
            {
                return true;
            }
            else
            {
                error = $"There only is room for {maybeTofu}.";
                return false;
            }
        }

        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall)
        {
            if (other is null)
            {
                sender.Reply($"You feel really warm.");
                return true;
            }
            else if (other == Game.EntryHall.Splinters)
            {
                sender.Inventory.Remove(other.Name);
                Burning = true;
                sender.Reply($"The smoldering ash is hot enough to make the splinters catch fire. The fire is burning again.");
                return true;
            }
            else
            {
                if (Burning)
                {
                    sender.Reply($"Your {other.Name} is now really warm.");
                }
                else
                {
                    sender.Reply($"The fire is not warm enough to do anything.");
                }
                return true;
            }
        }
    }
}
