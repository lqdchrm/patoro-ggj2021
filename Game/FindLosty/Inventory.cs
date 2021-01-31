using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class Inventory : Dictionary<string, Item>
    {
        public Item Transfer(string itemKey, Inventory target)
        {
            if (itemKey == null || !ContainsKey(itemKey) || target == null) return null;

            Item item = null;
            if (Remove(itemKey, out item))
            {
                target.Add(itemKey, item);
            }
            return item;
        }

        public bool Create(string name, string emoji, string desc)
        {
            if (!ContainsKey(name))
            {
                Add(name, new Item(name, emoji, desc));
                return true;
            }
            return false;
        }
    }
}
