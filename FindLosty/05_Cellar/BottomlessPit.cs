using LostAndFound.Engine;
using LostAndFound.FindLosty;

public class BottomlessPit : Item
{
    public override string Emoji => Emojis.Dog;


    public BottomlessPit(FindLostyGame game) : base(game)
    {
    }

    public override string Description {
        get {
            return $"You stare into the abyss, but there is nothing but blackness.\nYou hear a faint voice.";
        }
    }

    public override void Listen(IPlayer player)
    {
        if (!Game.Cellar.LightSwitch.IsOn)
        {
            player.Reply("You hear a thousand voices at once. 'MAYbE TrY tHe {LightSwitch} IdIOt'");
        }
        else
        {
            player.Reply(OneOf($"A voice echos in your mind. '{Game.Cellar.Losty}, give {Game.Cellar.Losty}, give {Game.Cellar.Losty} to us.'",
                               $"If you want to live you need to give us {Game.Cellar.Losty}. You really start to feel an urge to jump into the {this}"));
        }
    }

    public override void Use(IPlayer sender, IThing other)
    {
        if (other is BottomlessPit pit)
        {
            pit.ThrowInLosty(sender);
        }
        else
        {
            base.Use(sender, other);
        }
    }

    public void ThrowInLosty(IPlayer player)
    {
        Game.BroadcastMsg($"You see that {player} is about to throw {Game.Cellar.Losty} into the pit.\nYou feel an immediate sense of threat but there is nothing you can do.\n" +
                          $"{Game.Cellar.Losty} flys into the {this}. The {this} looks strangely happy. {Game.Cellar.Losty} does not.\n" +
                          $"As soon as {Game.Cellar.Losty} disapears a {Game.Cellar.Ladder} gets spit out of the {this}.");
    }
}
