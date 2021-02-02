using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty.Things
{
    public class Shelves : Container
    {

        bool DoorsOpen = false;
        
        public Shelves(FindLostyGame game) : base(game)
        {
        }

        public override string LookText(Thing other)
        {
            string message = "";
            if (other == null)
                return $"Some shelves.";
            return $"This is the best shelf ever";
        }

        public override string UseText(Thing other)
        {
            return $"This is the best shelf ever";
        }

        public override bool Use(Player player, Thing other)
        {
            var txt = UseText(other);

            player.SendTextWithState(txt);
            return true;
        }
    }
}
