using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty.Things
{
    public class Fridge : Container
    {
        public Fridge(FindLostyGame game) : base(game, false, "Fridge")
        {
            this.Inventory.InitialAdd(new Tofu(game));
        }


        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall)
        {
            var txt = this.UseText;

            sender.Reply(txt);
            return true;
        }
    }
}
