namespace LostAndFound.FindLosty.Things
{
    public class Microwave : Container
    {

        public Microwave(FindLostyGame game) : base(game, false, "Microwave")
        {
        }


        public override bool DoesItemFit(IThing thing, out string error)
        {
            IThing item = null;
            bool found_tofu = this.Inventory.TryFind("tofu", out item);
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

        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall)
        {
            IThing item = null;
            bool found_tofu = this.Inventory.TryFind("tofu", out item);
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
