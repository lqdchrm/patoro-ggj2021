using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Engine
{
    public interface BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>, IEnumerable<TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        bool CanAcceptNonTransferables { get; }
        bool DoesItemFit(TThing thing, out string error);
        bool Transfer(TThing thing, TContainer target);
        bool TryFind(string token, out TThing item, bool includeNextLevel = false, bool onlyWhenMentioned = true);

        bool Add(params TThing[] things);
        void Remove(TThing thing);
        bool Has(TThing thing, bool onlyWhenMentioned = true);

        void PutIntoThis(TPlayer sender, TThing thing);
    }

    public abstract class BaseContainerImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseThingImpl<TGame, TPlayer, TRoom, TContainer, TThing>, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        
        private readonly Dictionary<string, TThing> dict = new Dictionary<string, TThing>();
        public virtual bool CanAcceptNonTransferables => false;
        private static string _BuildKey(string itemName) => itemName.ToLowerInvariant();
        private static string _BuildKey(TThing item) => _BuildKey(item.Name);

        #region IEnumerable
        public IEnumerator<TThing> GetEnumerator() => this.dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.dict.Values.Where(i => i.WasMentioned).GetEnumerator();
        #endregion

        public BaseContainerImpl(TGame game, string name = null) : base(game, name) { }

        public virtual bool DoesItemFit(TThing thing, out string error) { error = ""; return true; }

        public bool Transfer(TThing thing, TContainer target) => Transfer(thing.Name, target);

        private bool Transfer(string name, TContainer target)
        {
            var key = _BuildKey(name);
            if (!this.dict.ContainsKey(key) || target == null) return false;

            if (this.dict.TryGetValue(key, out TThing item))
            {
                if (item.WasMentioned && (target.CanAcceptNonTransferables || item.CanBeTransfered))
                {
                    if (target.Add(item))
                    {
                        this.dict.Remove(key);
                        return true;
                    }
                }
            }
            return false;
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
                        if (container.TryFind(token, out item))
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
                item = default;
                return false;
            }
            return true;
        }

        public bool Add(params TThing[] things)
        {
            var result = true;
            foreach (var thing in things)
            {
                if (CanAcceptNonTransferables || thing.CanBeTransfered)
                {
                    var key = _BuildKey(thing.Name);
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, thing);
                        result &= true;
                    }
                } else
                {
                    result &= false;
                }
            }
            return result;
        }

        public void Remove(TThing thing) => Remove(thing.Name);
        private void Remove(string name)
        {
            var key = _BuildKey(name);
            this.dict.Remove(key);
        }

        public bool Has(TThing thing, bool onlyWhenMentioned = true) => Has(thing.Name, onlyWhenMentioned);
        private bool Has(string token, bool onlyWhenMentioned = true)
        {
            var key = _BuildKey(token);
            return this.dict.ContainsKey(key) && (!onlyWhenMentioned || this.dict[key].WasMentioned);
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override void Look(TPlayer sender)
        {
            // triggers WasMentioned
            var description = Description?.ToString() ?? "";

            var content = this;
            var contentText = content.Any() ? $"\t\nContaining: \n\t{string.Join("\n\t", content.Select(i => i.ToString()))}" : "";
            sender.Reply($"{description}{contentText}");
        }

        /*
         ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
         ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
         ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
         ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
         ███████╗██║███████║   ██║   ███████╗██║ ╚████║
         ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
         */
        public override string Noises => string.Join("\n", this.Select(i => i.Noises).Where(i => i != null));

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */
        public virtual void PutIntoThis(TPlayer sender, TThing thing)
        {
            if (sender == this || thing == this)
            {
                sender.Reply(
                    OneOf($"Not really.")
                );
            }
            else if (!DoesItemFit(thing, out string error))
            {
                sender.ReplyWithState(error);
            }
            else if (sender.Transfer(thing, this as TContainer))
            {
                if (this is TRoom)
                {
                    sender.ReplyWithState($"You drop {thing} in {this}");
                    sender.Room.BroadcastMsg($"{sender} dropped {thing} in {this}", sender);
                }
                else if (this is TPlayer otherPlayer)
                {
                    sender.ReplyWithState($"You give {thing} to {this}");
                    otherPlayer.Reply($"{sender} gave {thing} to you");
                    Game.BroadcastMsg($"{sender} gave {thing} to {this}", sender, otherPlayer);
                }
                else
                {
                    sender.ReplyWithState(OneOf($"You put {thing} into {this}", $"You packed {thing} into {this}."));
                    sender.Room.BroadcastMsg($"{sender} put {thing} into {this}", sender);
                }
            }
            else
            {
                sender.Reply(OneOf($"You don't have {thing}", $"{thing} not found in inventory."));
            }
        }

        public override void Use(TPlayer sender, TThing other)
        {
            if (other != null)
            {
                PutIntoThis(sender, other);
            } else
                base.Use(sender);
        }

    }
}
