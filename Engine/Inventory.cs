using LostAndFound.FindLosty;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Engine
{
    public class Inventory<TGame, TPlayer, TRoom, TContainer, TThing> : IEnumerable<TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        private readonly TContainer Owner;
        private bool canAcceptNonTransferables;
        private Dictionary<string, TThing> dict = new Dictionary<string, TThing>();

        #region IEnumerable
        public IEnumerator<TThing> GetEnumerator() => this.dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        #endregion

        #region Helpers
        private static string _BuildKey(string itemName) => itemName.ToLowerInvariant();
        private static string _BuildKey(TThing item) => _BuildKey(item.Name);
        #endregion

        public Inventory(TContainer owner, bool canAcceptNonTransferables = true)
        {
            this.Owner = owner;
            this.canAcceptNonTransferables = canAcceptNonTransferables;
        }

        public void InitialAdd(params TThing[] items)
        {
            foreach (var item in items)
            {
                if (this.canAcceptNonTransferables || item.CanBeTransfered)
                {
                    var key = _BuildKey(item);

                    if (!this.dict.ContainsKey(key))
                    {
                        this.dict.Add(key, item);
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Item {item} cannot be put into Inventory {this.Owner}");
                }
            }
        }

        public bool Transfer(string name, Inventory<TGame, TPlayer, TRoom, TContainer, TThing> target)
        {
            var key = _BuildKey(name);
            if (!this.dict.ContainsKey(key) || target == null) return false;

            TThing item;
            if (this.dict.TryGetValue(key, out item))
            {
                if (item.WasMentioned && item.CanBeTransfered)
                {
                    this.dict.Remove(key);
                    target.dict.Add(key, item);
                    return true;
                }
            }
            return true;
        }

        public bool TryFind(string token, out TThing item, bool includeNextLevel = false, bool onlyWhenMentioned = true)
        {
            var key = _BuildKey(token);
            if (!this.dict.TryGetValue(key, out item))
            {
                if (includeNextLevel)
                {
                    foreach (var container in this.OfType<TContainer>())
                    {
                        if (container.Inventory.TryFind(token, out item))
                            break;
                    }
                }
            }
            return CheckItem(ref item, onlyWhenMentioned);
        }

        private bool CheckItem(ref TThing item, bool onlyWhenMentioned = true)
        {
            if (item is null) return false;

            if (onlyWhenMentioned && !item.WasMentioned)
            {
                item = default(TThing);
                return false;
            }
            return true;
        }

        public void Remove(IThing thing) => Remove(thing.Name);
        public void Remove(string name)
        {
            var key = _BuildKey(name);
            this.dict.Remove(key);
        }

        public bool Has(IThing thing, bool onlyWhenMentioned = true) => Has(thing.Name, onlyWhenMentioned);
        public bool Has(string token, bool onlyWhenMentioned = true)
        {
            var key = _BuildKey(token);
            return (this.dict.ContainsKey(key) && (!onlyWhenMentioned || this.dict[key].WasMentioned));
        }
    }
}
