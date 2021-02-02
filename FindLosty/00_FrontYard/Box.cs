using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._00_FrontYard
{
    public class Box : Container
    {
        public override string Emoji => Emojis.Box;

        public Box(FindLostyGame game) : base(game)
        {
        }
    }
}
