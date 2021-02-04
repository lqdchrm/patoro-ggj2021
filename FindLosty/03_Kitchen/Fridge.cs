namespace LostAndFound.FindLosty._03_Kitchen
{
    public class Fridge : Container
    {
        public Tofu Tofu { get; }
        public Fridge(FindLostyGame game) : base(game, false, "Fridge")
        {
            Tofu = new Tofu(game);
            this.Inventory.InitialAdd(Tofu);
        }


        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall)
        {
            var txt = this.UseText;

            sender.Reply(txt);
            return true;
        }
    }
}
