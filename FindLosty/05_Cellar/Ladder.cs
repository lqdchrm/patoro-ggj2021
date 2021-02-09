using Patoro.TAE;


namespace FindLosty._05_Cellar
{
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
                hole.Escape(sender);
            }
            else if (other is BottomlessPit pit)
            {
                sender.Reply("You hear a growling voice 'Are you sure? We just gave this to you.'");
                sender.TheThingThatAskedAQuestion = this;
            }
            else
            {
                base.Use(sender, other);
            }
        }


        public override void answer(IPlayer player, string yes_or_no)
        {
            if (player.TheThingThatAskedAQuestion == this)
            {
                if (yes_or_no == "yes")
                {
                    player.Reply("You drop the {this} into the {Game.Cellar.BottomlessPit}.");
                    Game.Cellar.BottomlessPit.Remove(this);
                }
                else
                {
                    player.Reply("You feel like you just made a smart choice.");
                }
            }
            player.TheThingThatAskedAQuestion = null;
        }

    }
}