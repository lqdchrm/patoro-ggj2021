using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * ASCII HELPERS (Font used: ANSI Shadow)
 * http://patorjk.com/software/taag/#p=testall&f=Merlin2&t=LOOK
 * 
 * VSCode Extension: 
 * https://marketplace.visualstudio.com/items?itemName=BitBelt.converttoasciiart
*/

namespace LostAndFound.Engine
{
    public abstract class BaseThing<TGame, TRoom, TPlayer, TThing>
        where TGame : BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom : BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer : BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing : BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        public TGame Game { get; }
        public string Name { get; init; }
        public virtual string Emoji => "";
        public virtual bool IsVisible { get; set; } = true;

        public bool CanBeTransfered { get; init; }

        public BaseThing(TGame game, bool transferable = false, string name = null)
        {
            this.Game = game;
            this.Name = name ?? GetType().Name;
            this.CanBeTransfered = transferable;
        }

        public override string ToString() => $"[{Emoji}{Name}]";

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public virtual string LookText => OneOf($"It's {a} {this}", $"Nice, {a} {this}");
        public virtual void Look(TPlayer sender) => sender.Reply(LookText);

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public virtual string KickText => OneOf($"{this} was kicked.");
        private string KickSelf => OneOf($"You kicked yourself. WFT!");
        public virtual void Kick(TPlayer sender) => sender.Reply(this == sender ? KickSelf : KickText);


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        public virtual string ListenText => OneOf($"Nothing to hear from {this}.", $"... ... ...");
        public virtual void Listen(TPlayer sender) => sender.Reply(ListenText);

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        public virtual string OpenText => OneOf($"You can't open {this}.");
        public virtual void Open(TPlayer sender) => sender.Reply(OpenText);

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

        public virtual string CloseText => OneOf($"You can't close {this}.");
        public virtual void Close(TPlayer sender) => sender.Reply(CloseText);

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */

        public virtual string TakeText => OneOf($"You can't TAKE THAT™.");
        public virtual void Take(TPlayer sender, BaseThing<TGame, TRoom, TPlayer, TThing> other)
        {
            if (other is BaseContainer<TGame, TRoom, TPlayer, TThing> container)
                TakeFrom(sender, container);
            else
                sender.Reply(TakeText);
        }

        public virtual void TakeFrom(TPlayer sender, BaseContainer<TGame, TRoom, TPlayer, TThing> container)
        {
            if (!this.CanBeTransfered)
            {
                sender.Reply(OneOf($"{this} can't be taken. How could this even work?"));
            }
            else if (container.Inventory.Transfer(this.Name, sender.Inventory))
            {
                sender.ReplyWithState(OneOf($"You took {this} from {container}", $"Taken: {this}"));
                sender.Room.SendText($"{sender} now owns {this}");
            }
            else
            {
                sender.Reply(OneOf($"There is no {this}", $"{this} not found"));
            }
        }

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */
        public virtual string PutText => $"You can't put that here.";
        public virtual void Put(TPlayer sender, BaseThing<TGame, TRoom, TPlayer, TThing> other)
        {
            if (other is BaseContainer<TGame, TRoom, TPlayer, TThing> container)
                PutInto(sender, container);
            else
                sender.Reply(PutText);
        }

        public virtual void PutInto(TPlayer sender, BaseContainer<TGame, TRoom, TPlayer, TThing> container)
        {
            if (sender.Inventory == container.Inventory || this == container)
            {
                sender.Reply($"Not really.");
            } else if (sender.Inventory.Transfer(this.Name, container.Inventory))
            {
                sender.ReplyWithState(OneOf($"You put {this} into {container}", $"You crammed {this} into {container}."));
                sender.Room.SendText($"{this} is now in {container}", sender);
            }
            else
            {
                sender.Reply(OneOf($"You don't have {this}", $"{this} not found in inventory."));
            }
        }

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */
        public virtual string UseText => OneOf($"That won't work.", $"Really?");
        public virtual void Use(TPlayer sender, BaseThing<TGame, TRoom, TPlayer, TThing> other) => sender.Reply(UseText);


        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */
        protected static readonly string[] VOCALS = new[] { "A", "E", "I", "O", "U" };
        protected static string OneOf(params string[] texts) => texts.TakeOneRandom();
        protected string a => VOCALS.Contains(Char.ToUpper(Name.First()).ToString()) ? "an" : "a";

    }
}
