using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class Inventory : Dictionary<string, string>
    {
        public string Transfer(string itemKey, Inventory target)
        {
            if (itemKey == null || !ContainsKey(itemKey) || target == null) return null;

            string item = null;
            if (Remove(itemKey, out item))
            {
                target.Add(itemKey, item);
            }
            return item;
        }
    }
}
