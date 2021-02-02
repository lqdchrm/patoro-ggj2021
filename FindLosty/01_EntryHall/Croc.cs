using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Croc : Thing
    {
        public override string Emoji => Emojis.Croc;

        public Croc(FindLostyGame game) : base(game)
        {
        }
    }
}
