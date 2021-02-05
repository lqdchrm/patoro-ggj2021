using DSharpPlus.Entities;
using LostAndFound.Engine;
using System.Linq;

namespace LostAndFound.FindLosty
{
    public interface IPlayer : BasePlayer<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer
    {
        bool OmniPotentPowerOfShelf {get; set;}
        int Health { get; set; }
        bool Hit(string by = null, IPlayer byPlayer = null, int damage = 1);
        bool Heal(string by = null, IPlayer byPlayer = null);
    }

    public class Player : BasePlayerImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IPlayer
    {
        public Player(FindLostyGame game, string name) : base(game, name) { }

        private bool omniPotentPowerOfShelf = false;

        public bool OmniPotentPowerOfShelf {
            get => this.omniPotentPowerOfShelf;
            set
            {
                this.omniPotentPowerOfShelf = value;
            }
        }
        /*
         ███████╗████████╗ █████╗ ████████╗███████╗
         ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
         ███████╗   ██║   ███████║   ██║   █████╗  
         ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
         ███████║   ██║   ██║  ██║   ██║   ███████╗
         ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
         */
        public override string StatusText
        {
            get
            {
                var health = string.Join("", Enumerable.Repeat(Emojis.Heart, this.Health));
                var items = string.Join("", this.Inventory.Select(i => i.Emoji));
                var item = this.ThingPlayerIsUsingAndHasToStop?.ToString();
                var action = item != null ? $"using {item}" : "";
                var super_powers_start =  OmniPotentPowerOfShelf ? "🔥🌎🔥":"";
                var super_powers_end = OmniPotentPowerOfShelf ? "🔥🌎🔥":"";
                return $"[{super_powers_start} {this.Emoji}{this.Name} {super_powers_end}] {health} {items} {action}";
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
        public override string LookTextHeader
        {
            get
            {
                var adj = OneOf("beautiful", "scary", "awesome", "strange", "marvelous", "anxious", "fabulous");
                return $"{this} looks {adj}.";
            }
        }

        public override void Look(IPlayer sender)
        {
            base.Look(sender);
            var verb = OneOf("staring at", "looking at", "admiring", "peeping on");
            Reply($"{sender} is {verb} you.");
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
            base.Kick(sender);
            var how = OneOf("hard", "in your butt", "with love", "and you deserved it");
            Reply($"{sender} kicked you {how}");
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
            base.Listen(sender);
            Reply($"{sender} is listening to you...");
        }

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

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

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */

        public bool Hit(string by = null, IPlayer byPlayer = null, int damage = 1)
        {
            if (this.Health > 0)
            {
                this.Health = System.Math.Max(this.Health - damage, 0);



                var msg = "You were hit";

                if (by != null)
                {
                    msg += $" by {by}";
                }
                else if (byPlayer != null)
                {
                    byPlayer.Reply($"You hit [{this}] really hard.");
                    msg += $" by [{byPlayer}], but it was probably deserved";
                }

                if(Health == 0)
                {
                    msg += " (Your dead)";
                }

                ReplyWithState(msg);
                return true;
            }
            else
            {
                if (byPlayer != null)
                {
                    byPlayer.Reply("Why are you hitting dead people?");
                    byPlayer.Room.SendText($"[{byPlayer}] is hitting dead [{this}]. Give a big BOOOO.....", byPlayer);
                }
            }
            return false;
        }

        public bool Heal(string by = null, IPlayer byPlayer = null)
        {
            if (this.Health < PLAYER_MAX_HEALTH)
            {
                this.Health++;

                var msg = "You were healed";

                if (by != null)
                {
                    msg += $" by {by}";
                }
                else if (byPlayer != null)
                {
                    byPlayer.Reply($"You healed [{this}].");
                    msg += $" by [{byPlayer}]. You really got some friends here.";
                }

                ReplyWithState(msg);
                return true;
            }
            else
            {
                Reply("You need no healing.");
                return false;
            }
        }
    }
}
