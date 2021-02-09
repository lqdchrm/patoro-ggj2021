namespace FindLosty
{
    public abstract class Item : Thing
    {
        public override bool CanBeTransfered => true;

        public Item(FindLostyGame game, string name = null) : base(game, name) { }
    }
}
