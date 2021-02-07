using LostAndFound.Engine;
using LostAndFound.FindLosty;

public class Ladder : Item
{
    public override string Emoji => Emojis.Dog;

    public Ladder(FindLostyGame game) : base(game)
    {
    }

    public override string Description => @"A big ladder.";

    public override void Use(IPlayer sender, IThing other)
    {
        if (other is Hole hole)
        {
            hole.Escape();
        }
        else
        {
            base.Use(sender, other);
        }
    }

}
