using LostAndFound.Engine;
using LostAndFound.FindLosty;

public class Hole : Item
{
    public override string Emoji => Emojis.Dog;

    public Hole(FindLostyGame game) : base(game)
    {
    }

    public override string Description => @"A small hole in the ceilling. It's not reachable without a ladder or rope.";


    public override void Use(IPlayer sender, IThing other)
    {
        if (other is Ladder)
        {
        }
        else
        {
            base.Use(sender, other);
        }
    }

    public void Escape()
    {
        Game.BroadcastMsg($"You climb up the ladder and escape to safety.\n\n" +
                          $"Thanks for playing. You can find the source code for this game at https://github.com/lqdchrm/patoro-ggj2021\n" +
                          $"\n"+
                          $"A game by:\n"+
                          $"LokiMidgard     (https://github.com/LokiMidgard)\n" +
                          $"lqdchrm         (https://github.com/lqdchrm)\n" +
                          $"Tobias Krumholz (https://github.com/krumholt)\n");
    }
}
