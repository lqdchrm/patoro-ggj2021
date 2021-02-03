using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public abstract class BaseContainer<TGame, TRoom, TPlayer, TThing> : BaseThing<TGame, TRoom, TPlayer, TThing>
        where TGame : BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom : BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer : BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        internal Inventory<TGame, TRoom, TPlayer, TThing> Inventory { get; init; }

        public BaseContainer(TGame game, bool transferable, bool canAcceptNonTransferables = false, string name = null) : base(game, transferable, name)
        {
            Inventory = new Inventory<TGame, TRoom, TPlayer, TThing>(this, canAcceptNonTransferables);
        }

        public virtual bool DoesItemFit(BaseThing<TGame, TRoom, TPlayer, TThing> thing, out string error)
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
        public virtual string LookInventoryText => "\nInside:\n\t" + string.Join("\n\t", Inventory.Select(i => i.ToString()));
        public override string LookText => $"{LookTextHeader}{LookInventoryText}";
    }
}
