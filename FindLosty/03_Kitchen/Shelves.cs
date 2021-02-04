using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class Shelves : Container
    {

        bool DoorsOpen = false;
        
        public Shelves(FindLostyGame game) : base(game, false, "Shelves")
        {

        }

        public override string LookText
        {
            get {
                return $"This is the best shelf ever";
            }
        }

        public override string UseText
        {
            get {
                return $"This is the best shelf ever";
            }
        }

        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall)
        {
            string txt = UseText;

            sender.Reply("YOU JUST USED ME. POLICE." + txt);
            return true;
        }
    }
}
