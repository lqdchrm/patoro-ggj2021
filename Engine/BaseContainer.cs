using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        Inventory<TGame, TPlayer, TRoom, TContainer, TThing> Inventory { get; init; }
        bool DoesItemFit(TThing thing, out string error);
    }

    public abstract class BaseContainerImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseThingImpl<TGame, TPlayer, TRoom, TContainer, TThing>, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public Inventory<TGame, TPlayer, TRoom, TContainer, TThing> Inventory { get; init; }

        public BaseContainerImpl(TGame game, bool transferable, bool canAcceptNonTransferables = false, string name = null) : base(game, transferable, name)
        {
            this.Inventory = new Inventory<TGame, TPlayer, TRoom, TContainer, TThing>(this as TContainer, canAcceptNonTransferables);
        }

        public virtual bool DoesItemFit(TThing thing, out string error)
        {
            error = "";
            return true;
        }
        
        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */

        public virtual string LookTextHeader => base.LookText;
        public virtual string LookInventoryText => "\nInside:\n\t" + string.Join("\n\t", this.Inventory.Select(i => i.ToString()));
        public override string LookText => $"{this.LookTextHeader}{this.LookInventoryText}";
    }
}
