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
        bool WasMentioned { get; set; }
        bool CanBeTransfered { get; }

        string Description { get; }
        void Look(TPlayer sender);

        void Kick(TPlayer sender);

        string Noises { get; }
        void Listen(TPlayer sender);

        void Open(TPlayer sender);

        void Close(TPlayer sender);

        void Take(TPlayer sender, TThing thing);
        void TakeFrom(TPlayer sender, TContainer container);

        void Put(TPlayer sender, TThing thing);
        void PutInto(TPlayer sender, TContainer container);

        void Use(TPlayer sender);
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
        public virtual bool WasMentioned { get; set; } = false;
        public virtual bool CanBeTransfered => false;

        public BaseThingImpl(TGame game, string name = null)
        {
            this.Game = game;
            this.Name = name ?? GetType().Name;
        }

        public override string ToString()
        {
            this.WasMentioned = true;
            return Game.FormatThing(this as TThing);
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public virtual string Description => OneOf(
            $"It's {a} {this}",
            $"Nice, {a} {this}"
        );

        public virtual void Look(TPlayer sender)
        {
            if (Description is not null)
                sender.Reply(Description);
        }

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public virtual void Kick(TPlayer sender) =>
            sender.Reply(
                OneOf($"You kicked {this}.")
            );


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */
        public virtual string Noises => OneOf(
                $"Nothing to hear from {this}.",
                $"... ... ..."
        );
        
        public virtual void Listen(TPlayer sender)
        {
            if (Noises is not null)
                sender.Reply(Noises);
        }

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */
        public virtual void Open(TPlayer sender) =>
            sender.Reply(
                OneOf($"You can't open {this}.")
            );

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */
        public virtual void Close(TPlayer sender) =>
            sender.Reply(
                OneOf($"You can't close {this}.")
            );

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */
        public void Take(TPlayer sender, TThing other)
        {
            if (other is not TContainer container)
                sender.Reply(
                    OneOf($"You can't TAKE THAT™.")
                );
            else 
                TakeFrom(sender, container);
        }

        public virtual void TakeFrom(TPlayer sender, TContainer container)
        {
            if (!this.CanBeTransfered)
            {
                sender.Reply(OneOf($"{this} can't be taken. How could this even work?"));
            }
            else if (container.Transfer(this as TThing, sender))
            {
                sender.ReplyWithState(OneOf($"You took {this} from {container}", $"Taken: {this}"));
                if (container is TPlayer otherPlayer)
                {
                    sender.Room.BroadcastMsg($"{sender} stole {this} from {container}", sender, otherPlayer);
                    otherPlayer.Reply($"{sender} has stolen {a} {this} from you.");
                }
                else
                {
                    sender.Room.BroadcastMsg($"{sender} took {this} out of {container}", sender);
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
        public void Put(TPlayer sender, TThing other)
        {
            if (other is not TContainer container)
                sender.Reply(
                    OneOf($"You can't put that here.")
                );
            else
                container.PutIntoThis(sender, this as TThing);
        }

        public virtual void PutInto(TPlayer sender, TContainer container)
        {
            if (sender == container || this == container)
            {
                sender.Reply(
                    OneOf($"Not really.")
                );
            }
            else if (!container.DoesItemFit(this as TThing, out string error))
            {
                sender.ReplyWithState(error);
            }
            else if (sender.Transfer(this as TThing, container))
            {
                if (container is TRoom)
                {
                    sender.ReplyWithState($"You drop {this} in {container}");
                    sender.Room.BroadcastMsg($"{sender} dropped {this} in {container}", sender);
                }
                else if (container is TPlayer otherPlayer)
                {
                    sender.ReplyWithState($"You give {this} to {container}");
                    otherPlayer.Reply($"{sender} gave {this} to you");
                    Game.BroadcastMsg($"{sender} gave {this} to {container}", sender, otherPlayer);
                }
                else
                {
                    sender.ReplyWithState(OneOf($"You put {this} into {container}", $"You packed {this} into {container}."));
                    sender.Room.BroadcastMsg($"{sender} put {this} into {container}", sender);
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
        public virtual void Use(TPlayer sender) =>
            sender.Reply(
                 OneOf(
                     $"That won't work.",
                     $"Really?"
                 ));

        public virtual void Use(TPlayer sender, TThing other) =>
            sender.Reply(
                OneOf(
                    $"You can't use {this} with {other}.",
                    $"It's not a good idea to use {this} with {other}."
                ));

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
        protected string A => VOCALS.Contains(Char.ToUpper(this.Name.First()).ToString()) ? "An" : "A";
        protected string a => A.ToLowerInvariant();
    }
}
