namespace LostAndFound.FindLosty
{
    public abstract class Item : Thing
    {
        public Item(FindLostyGame game) : this(game, null) { }
        public Item(FindLostyGame game, string name) : base(game, true, name) { }
    }
}
