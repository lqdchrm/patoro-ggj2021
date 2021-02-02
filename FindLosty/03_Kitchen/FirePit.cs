using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class FirePit : Thing
    {
        bool Burning = false;
        
        public FirePit(FindLostyGame game) : base(game)
        {
            Name = "pit";
        }


        public override string LookText
        {
            get {
                if (Burning)
                    return $"The fire is blazing.";
                else
                    return $"The fire is out. The ash is still smoldering.";
            }
        }
        

        public override bool Use(Player sender, BaseThing<FindLostyGame, Room, Player, Thing> other, bool isFlippedCall)
        {
            if (other is null)
            {
                sender.Reply($"You feel really warm.");
                return true;
            }
            else
            {
                sender.Reply($"You throw {other.Name} into the fire.");
                return true;
            }
        }
    }
}
