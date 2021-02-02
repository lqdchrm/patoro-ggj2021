using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Phone : Thing
    {
        public override string Emoji => Emojis.Phone;

        public Phone(FindLostyGame game) : base(game)
        {
        }
    }
}
