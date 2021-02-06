using DSharpPlus.Entities;
using LostAndFound.Engine;
using System.Linq;

namespace LostAndFound.FindLosty
{
    public interface IPlayer : BasePlayer<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer
    {
        bool OmniPotentPowerOfShelf {get; set;}
        int Health { get; set; }
        bool Hit(int damage = 1);
    }

    public class Player : BasePlayerImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IPlayer
    {
        public Player(FindLostyGame game, string name) : base(game, name) { }

        /*
         ███████╗████████╗ █████╗ ████████╗███████╗
         ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
         ███████╗   ██║   ███████║   ██║   █████╗  
         ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
         ███████║   ██║   ██║  ██║   ██║   ███████╗
         ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
         */
        public bool OmniPotentPowerOfShelf { get; set; }

        public override string StatusText
        {
            get
            {
                var health = string.Join("", Enumerable.Repeat(Emojis.Heart, this.Health));
                var items = string.Join("", this.Select(i => i.Emoji));
                var item = this.ThingPlayerIsUsingAndHasToStop?.ToString();
                var action = item != null ? $"using {item}" : "";
                var super_powers =  OmniPotentPowerOfShelf ? " and has the 🔥🌎🔥omnipotent power of shelf🔥🌎🔥" : "";
                return $"[{this.Emoji}{this.Name}] {health} {items} {action} {super_powers}";
            }
        }

        /*
        ██╗  ██╗███████╗ █████╗ ██╗  ████████╗██╗  ██╗
        ██║  ██║██╔════╝██╔══██╗██║  ╚══██╔══╝██║  ██║
        ███████║█████╗  ███████║██║     ██║   ███████║
        ██╔══██║██╔══╝  ██╔══██║██║     ██║   ██╔══██║
        ██║  ██║███████╗██║  ██║███████╗██║   ██║  ██║
        ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚═╝   ╚═╝  ╚═╝
        */
        const int PLAYER_MAX_HEALTH = 5;

        private int health = PLAYER_MAX_HEALTH;

        public int Health
        {
            get => this.health;
            set
            {
                var old = this.health;
                this.health = value;
                if (value == 0 && old > 0)
                {
                    ReplyWithState($"You health has depleted and you are now muted. Try to heal yourself to be able to speak again (or ask the server admin to unmute you).");
                    Mute();
                }
                else if (old == 0 && value > 0)
                {
                    ReplyWithState($"You health was restored and you are now unmuted.");
                    Unmute();
                }
            }
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override void Look(IPlayer sender)
        {
            var adjs = new[] { "nice", "beautiful", "scary", "awesome", "strange", "marvelous", "anxious", "fabulous" };

            if (this == sender)
            {
                sender.Reply($"You look {OneOf(adjs)})");
            } else {
                var header = $"It's {StatusText}, owning:\n\t";
                var items = string.Join("\n\t", this.Select(i => i.ToString()));
                sender.Reply($"{header}{items}");

                var verbs = new[] { "staring at", "looking at", "admiring", "peeping on" };
                Reply($"{sender} is {OneOf(verbs)} you.");
            }
        }

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public override void Kick(IPlayer sender)
        {
            if (this == sender)
            {
                sender.Reply($"You kicked yourself, Wee Man.");
            }
            else
            {
                var hows = new[] { "hard", "in the butt", "with no mercy", "where it hurts" };

                // to kicker
                sender.Reply($"You kicked {this} {OneOf(hows)}");

                // to kicked
                Hit();
                Reply($"{sender} kicked you {OneOf(hows)}");

                // to others
                sender.Room.BroadcastMsg($"{sender} kicked {this} {OneOf(hows)}", this, sender);
            }
        }


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */
        public override void Listen(IPlayer sender)
        {
            if (this == sender)
            {
                if (OmniPotentPowerOfShelf)
                {
                    sender.Reply($"You hear the power of million shelves.");
                }
                else
                {
                    sender.Reply($"You listen to your inner self.");
                }
            }
            else
            {
                var verbs = new[] { "is mumbling", "keeps silent" };
                sender.Reply($"{this} {OneOf(verbs)}");
                Reply($"{sender} is listening to you...");
            }
        }

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */
        public override void Open(IPlayer sender)
        {
            if (this == sender)
            {
                if (OmniPotentPowerOfShelf)
                {
                    sender.Reply($"You unleash the power of million shelves.");
                    Game.BroadcastMsg($"{sender} unleashed the power of million shelves.");
                }
                else
                {
                    sender.Reply($"You open up your mind.");
                }
            } else
            {
                base.Open(sender);
            }
        }

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */
        public override void Close(IPlayer sender)
        {
            if (sender == this)
            {
                sender.Reply("You straighten up.");
            } else
            {
                base.Close(sender);
            }
        }

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */
        public override void Use(IPlayer sender)
        {
            if (sender == this)
            {
                if (!sender.Room.Players.Skip(1).Any())
                {
                    sender.Reply("Nobody is here. So you start \"using\" yourself");
                } else
                {
                    sender.Reply("Here are other people. You are too shy to \"use\" yourself here.");
                }
            } else
            {
                sender.Reply($"{this} doesn't want to be \"used\".");
                Reply($"{sender} tried to use you.");
            }
        }

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */

        public bool Hit(int damage = 1)
        {
            if (this.Health > 0)
            {
                this.Health = System.Math.Max(this.Health - damage, 0);
                return true;
            }
            return false;
        }
    }
}
