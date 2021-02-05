using System;
using System.Linq;

/*
 * ASCII HELPERS (Font used: ANSI Shadow)
 * http://patorjk.com/software/taag/#p=testall&f=Merlin2&t=LOOK
 * 
 * VSCode Extension: 
 * https://marketplace.visualstudio.com/items?itemName=BitBelt.converttoasciiart
*/

namespace LostAndFound.Engine
{
    public interface BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        TGame Game { get; }
        string Name { get; init; }
        string Emoji { get; }
        bool IsVisible { get; }
        bool WasMentioned { get; set; }
        bool CanBeTransfered { get; init; }

        string LookText { get; }
        void Look(TPlayer sender);

        string KickText { get; }
        void Kick(TPlayer sender);

        string ListenText { get; }
        void Listen(TPlayer sender);

        string OpenText { get; }
        void Open(TPlayer sender);

        string CloseText { get; }
        void Close(TPlayer sender);

        string TakeText { get; }
        void Take(TPlayer sender, TThing other);
        void TakeFrom(TPlayer sender, TContainer container);

        string PutText { get; }
        void Put(TPlayer sender, TThing other);
        void PutInto(TPlayer sender, TContainer container);

        string UseText { get; }
        void Use(TPlayer sender, TThing other);
    }

    public abstract class BaseThingImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public TGame Game { get; }
        public string Name { get; init; }
        public virtual string Emoji => "";
        public virtual bool IsVisible { get; protected set; } = true;
        public virtual bool WasMentioned { get; set; } = false;

        public bool CanBeTransfered { get; init; }

        public BaseThingImpl(TGame game, bool transferable = false, string name = null)
        {
            this.Game = game;
            this.Name = name ?? GetType().Name;
            this.CanBeTransfered = transferable;
        }

        public override string ToString()
        {
            this.WasMentioned = true;
            return $"[{this.Emoji}{this.Name}]";
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public virtual string LookText => OneOf($"It's {this.a} {this}", $"Nice, {this.a} {this}");
        public virtual void Look(TPlayer sender) => sender.Reply(this.LookText);

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
        public virtual void Kick(TPlayer sender) => sender.Reply(this == sender ? this.KickSelf : this.KickText);


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        public virtual string ListenText => OneOf($"Nothing to hear from {this}.", $"... ... ...");
        public virtual void Listen(TPlayer sender) => sender.Reply(this.ListenText);

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        public virtual string OpenText => OneOf($"You can't open {this}.");
        public virtual void Open(TPlayer sender) => sender.Reply(this.OpenText);

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

        public virtual string CloseText => OneOf($"You can't close {this}.");
        public virtual void Close(TPlayer sender) => sender.Reply(this.CloseText);

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */

        public virtual string TakeText => OneOf($"You can't TAKE THAT™.");
        public virtual void Take(TPlayer sender, TThing other)
        {
            if (other is TContainer container)
                TakeFrom(sender, container);
            else
                sender.Reply(this.TakeText);
        }

        public virtual void TakeFrom(TPlayer sender, TContainer container)
        {
            if (!this.CanBeTransfered)
            {
                sender.Reply(OneOf($"{this} can't be taken. How could this even work?"));
            }
            else if (container.Inventory.Transfer(this as TThing, sender.Inventory))
            {
                sender.ReplyWithState(OneOf($"You took {this} from {container}", $"Taken: {this}"));
                if (container is TPlayer otherPlayer)
                {
                    sender.Room.SendText($"{sender} took {this} from {container}", sender, otherPlayer);
                    otherPlayer.Reply($"{sender} took {this} from you");
                }
                else
                {
                    sender.Room.SendText($"{sender} took {this} from {container}", sender);
                }
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
        public virtual void Put(TPlayer sender, TThing other)
        {
            if (other is TContainer container)
                PutInto(sender, container);
            else
                sender.Reply(this.PutText);
        }

        public virtual void PutInto(TPlayer sender, TContainer container)
        {
            if (sender.Inventory == container.Inventory || this == container)
            {
                sender.Reply($"Not really.");
            }
            else if (!container.DoesItemFit(this as TThing, out string error))
            {
                sender.ReplyWithState(error);
            }
            else if (sender.Inventory.Transfer(this as TThing, container.Inventory))
            {
                if (container is TRoom)
                {
                    sender.ReplyWithState($"You drop {this} in {container}");
                    sender.Room.SendText($"{sender} dropped {this} in {container}", sender);
                }
                else if (container is TPlayer otherPlayer)
                {
                    sender.ReplyWithState($"You give {this} to {container}");
                    otherPlayer.Reply($"{sender} gave {this} to you");
                    sender.Room.SendText($"{sender} gave {this} to {container}", sender, otherPlayer);
                }
                else
                {
                    sender.ReplyWithState(OneOf($"You put {this} into {container}", $"You crammed {this} into {container}."));
                    sender.Room.SendText($"{sender} put {this} into {container}", sender);
                }
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
        public virtual void Use(TPlayer sender, TThing other) => sender.Reply(this.UseText);

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
        protected string a => VOCALS.Contains(Char.ToUpper(this.Name.First()).ToString()) ? "an" : "a";

    }
}
