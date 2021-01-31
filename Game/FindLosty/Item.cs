using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class Item
    {
        public string Name { get;  }
        public string Emoji { get; }

        public string Description { get; }

        public Item(string name, string emoji, string desc)
        {
            this.Name = name;
            this.Emoji = emoji;
            this.Description = desc;
        }

        public override string ToString() => Emoji;
    }
}
