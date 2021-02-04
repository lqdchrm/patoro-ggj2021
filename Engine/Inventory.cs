using LostAndFound.FindLosty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class Inventory<TGame, TRoom, TPlayer, TThing> : IEnumerable<BaseThing<TGame, TRoom, TPlayer, TThing>>
        where TGame : BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom : BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer : BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing : BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        private readonly BaseContainer<TGame, TRoom, TPlayer, TThing> Owner;
        private bool canAcceptNonTransferables;
        private Dictionary<string, BaseThing<TGame, TRoom, TPlayer, TThing>> dict = new Dictionary<string, BaseThing<TGame, TRoom, TPlayer, TThing>>();

        #region IEnumerable
        public IEnumerator<BaseThing<TGame, TRoom, TPlayer, TThing>> GetEnumerator() => dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        #endregion

        #region Helpers
        private static string _BuildKey(string itemName) => itemName.ToLowerInvariant();
        private static string _BuildKey(BaseThing<TGame, TRoom, TPlayer, TThing> item) => _BuildKey(item.Name);
        #endregion

        public Inventory(BaseContainer<TGame, TRoom, TPlayer, TThing> owner, bool canAcceptNonTransferables = true)
        {
            this.Owner = owner;
            this.canAcceptNonTransferables = canAcceptNonTransferables;
        }

        public void InitialAdd(params BaseThing<TGame, TRoom, TPlayer, TThing>[] items)
        {
            foreach (var item in items)
            {
                if (canAcceptNonTransferables || item.CanBeTransfered)
                {
                    var key = _BuildKey(item);

                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, item);
                    }
                } else
                {
                    Console.Error.WriteLine($"Item {item} cannot be put into Inventory {Owner}");
                }
            }
        }

        public bool Transfer(string name, Inventory<TGame, TRoom, TPlayer, TThing> target)
        {
            var key = _BuildKey(name);
            if (!dict.ContainsKey(key) || target == null) return false;

            BaseThing<TGame, TRoom, TPlayer, TThing> item;
            if (dict.TryGetValue(key, out item))
            {
                if (item.WasMentioned && item.CanBeTransfered)
                {
                    dict.Remove(key);
                    target.dict.Add(key, item);
                    return true;
                }
            }
            return true;
        }

        public bool TryFind(string token, out BaseThing<TGame, TRoom, TPlayer, TThing> item, bool includeNextLevel = false, bool onlyWhenMentioned = true)
        {
            var key = _BuildKey(token);
            if (!dict.TryGetValue(key, out item))
            {
                if (includeNextLevel)
                {
                    foreach (var container in this.OfType<BaseContainer<TGame, TRoom, TPlayer, TThing>>())
                    {
                        if (container.Inventory.TryFind(token, out item))
                            break;
                    }
                }
            }
            return CheckItem(ref item, onlyWhenMentioned);
        }

        private bool CheckItem(ref BaseThing<TGame, TRoom, TPlayer, TThing> item, bool onlyWhenMentioned = true)
        {
            if (item is null) return false;

            if (onlyWhenMentioned && !item.WasMentioned)
            {
                item = null;
                return false;
            }
            return true;
        }

        public void Remove(string name)
        {
            var key = _BuildKey(name);
            dict.Remove(key);
        }

        public bool Has(string token, bool onlyWhenMentioned = true)
        {
            var key = _BuildKey(token);
            return (dict.ContainsKey(key) && (!onlyWhenMentioned || dict[key].WasMentioned));
        }
    }
}
