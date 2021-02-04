using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public abstract class Item : Thing {
        public Item(FindLostyGame game) : this(game, null) { }
        public Item(FindLostyGame game, string name) : base(game, true, name) { }
    }
}
