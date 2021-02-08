using LostAndFound.Engine;
using LostAndFound.FindLosty;
using LostAndFound.FindLosty._02_DiningRoom;

public class BottomlessPit : Container
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
        if (other is Losty losty)
        {
            ThrowInLosty(sender);
        }
        if (other is Hamster hamster)
        {
            if (hamster.power_of_chuck_norris)
            {
                sender.Reply($"You drop the {hamster} into the pit. You feel it's ready to fight the darkness once and for all\n" +
                             $"You can feel him harnessing his inner Chuck Norris. With a constant roundhouse kick he disapears into the {this}.\n" +
                             $"Only seconds later the {hamster} and a {Game.Cellar.Ladder} get spit out of the {this}.\n");
                Game.BroadcastMsg($"You see that {sender} throws the {hamster} into the pit. You think that's insane but at this point you don't really know.\n" +
                                  $"To your surprise seconds later the {hamster} and a {Game.Cellar.Ladder} get spit out of the {this}", sender);
            }
            else
            {
                sender.Reply($"You drop the {hamster} into the pit. You feel it's ready to fight the darkness once and for all\n" +
                             $"In the last moment you and the hamster realise something is missing. If only he could 'ward' himself\n" +
                             $"or had some kind of 'robe'\n");
                Game.BroadcastMsg($"You see that {sender} throws the {hamster} into the pit. You think that's insane but at this point you don't really know.", sender);
                hamster.WasMentioned = false;
            }
        }
        else
        {
            base.Use(sender, other);
        }
    }

    public override bool DoesItemFit(IThing item, out string error)
    {
        error = "Try 'use'ing it with the pit.";
        return false;
    }

    public void ThrowInLosty(IPlayer player)
    {
        string throw_in = $"{Game.Cellar.Losty} flys into the {this}. The {this} looks strangely happy. {Game.Cellar.Losty} does not.\n" +
            $"As soon as {Game.Cellar.Losty} disapears a {Game.Cellar.Ladder} gets spit out of the {this}.";
        player.Reply($"You take your best aim and hurl {Game.Cellar.Losty} towards the {this}.\n" + throw_in);
        Game.BroadcastMsg($"You see that {player} is about to throw {Game.Cellar.Losty} into the pit.\nYou feel an immediate sense of threat but there is nothing you can do.\n" +
                          throw_in, player);
    }
}
