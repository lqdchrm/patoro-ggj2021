using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Staircase : Thing
    {
        public override string Emoji => Emojis.Stairs;

        public Staircase(FindLostyGame game) : base(game)
        {
        }
    }
}
