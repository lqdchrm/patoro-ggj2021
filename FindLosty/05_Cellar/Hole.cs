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
            Escape(sender);
        }
        else if (other is Losty)
        {
            ThrowInLosty(sender);
        }
        else
        {
            base.Use(sender, other);
        }
    }

    public void ThrowInLosty(IPlayer sender)
    {
        sender.Reply($"You hear barking from outside. {Game.Cellar.Losty} must have made it out safely.\n\n" +
                     $"You are hit by the realisiation that saving {Game.Cellar.Losty} comes at price.\n" +
                     $"You take one last look at the {this} and wish {Game.Cellar.Losty} a long live\n" +
                     $"before you strap your chainsaw to your missing hand and jump into the pit\n" +
                     $"shouting: 'Groovy'.\n" +
                     $"\n" + 
                     credits);
        Game.BroadcastMsg($"You can't believe that sender just threw away your best chance at escaping.\n" +
                          $"{sender} straps his chainsaw to his missing hand and jumps into the pit\n" +
                          $"shouting: 'Groovy'.\n" +
                          $"You are hit by the realisiation that there might be no escape for you\n" +
                          $"\n" + 
                          credits, sender);
        Game.GameEnded = true;
    }

    public void Escape(IPlayer sender)
    {
        bool taken_losty_to_safety = false;
        foreach (Player p in Game.Players.Values)
        {
            if (p.Has(Game.Cellar.Losty))
            {
                taken_losty_to_safety = true;
            }
        }
        if (taken_losty_to_safety)
            GoodEscape(sender);
        else
            BadEscape(sender);
    }

    public void GoodEscape(IPlayer sender)
    {
        Game.BroadcastMsg($"You climb up the ladder and escape to safety.\n" +
                          $"{Game.Cellar.Losty} is safely with you. Your not sure where the hamster is now.\n" +
                          $"But you know he'll be alright.\n" +
                          $"Thanks for playing. You can find the source code for this game at https://github.com/lqdchrm/patoro-ggj2021\n" +
                          $"\n"+
                          credits);
        Game.GameEnded = true;
    }

    public void BadEscape(IPlayer sender)
    {
        Game.BroadcastMsg($"You climb up the {Game.Cellar.Ladder} and escape to safety.\n" +
                          $"You can still hear {Game.Cellar.Losty}s screams as he plummets into the abyss.\n" +
                          $"You try to think about what you might have done differently. But i guess it's too late now.\n" +
                          $"Thanks for playing. You can find the source code for this game at https://github.com/lqdchrm/patoro-ggj2021\n" +
                          $"\n"+
                          credits);
        Game.GameEnded = true;
    }

    private string credits = 
        $"Thanks for playing.\nYou can find the source code for this game at https://github.com/lqdchrm/patoro-ggj2021\n" +
        $"\n"+
        $"A game by:\n"+
        $"LokiMidgard     (https://github.com/LokiMidgard)\n" +
        $"lqdchrm         (https://github.com/lqdchrm)\n" +
        $"Tobias Krumholz (https://github.com/krumholt)\n";
}
