using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty.Things
{
    public class Fridge : Thing
    {
        public Fridge(FindLostyGame game) : base(game)
        {
        }


        public override string UseText(Thing other)
        {
            return $"This is the best fridge ever";
        }

        public override bool Use(Player player, Thing other)
        {
            var txt = UseText(other);

            player.SendTextWithState(txt);
            return true;
        }
    }
}
