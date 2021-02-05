using LostAndFound.Engine;
using System.Linq;

namespace LostAndFound.FindLosty._03_Kitchen
{
    public class Microwave : Container
    {
        public override string Emoji => Emojis.Microwave;

        public Microwave(FindLostyGame game) : base(game, false, "Microwave")
        {
        }

        /*
         ███████╗████████╗ █████╗ ████████╗███████╗
         ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
         ███████╗   ██║   ███████║   ██║   █████╗  
         ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
         ███████║   ██║   ██║  ██║   ██║   ███████╗
         ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
         */
        public bool IsOpen { get; private set; }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string LookText {
            get {
                var intro = $"You see an old { this}. The years or use have left stains all over it. You wouldn't want to put food in there.";

                var hasCord = this.Game.Kitchen.Inventory.Has(Game.Kitchen.Powercord, false);
                var cordText = hasCord ? $"\nThere's a long {this.Game.Kitchen.Powercord} dangling on the left side." : "";

                var openText = IsOpen ? "It is open." : "It is closed.";

                return $"{intro}{cordText}\n{openText}";
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


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

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
            if (!IsOpen)
            {
                sender.Reply($"You opened the {this}.");
                IsOpen = true;
            } else
            {
                sender.Reply($"The {this} is already open.");
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
            if (IsOpen)
            {
                sender.Reply($"You closed the {this}.");
                IsOpen = false;
            }
            else
            {
                sender.Reply($"The {this} is already firmly shut.");
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
        public override void Take(IPlayer sender, IThing other)
        {
            if (other is not null)
            {
                if (!IsOpen)
                {
                    sender.Reply($"The {this} is closed.");
                    return;
                }
            }
            base.Take(sender, other);
        }

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */
        public override bool DoesItemFit(IThing thing, out string error)
        {
            error = "";
            if (!IsOpen)
            {
                error = $"The {this} is closed";
                return false;
            }

            IThing thing_inside = this.Inventory.FirstOrDefault();
            if (thing is Tofu)
            {
                return true;
            }
            else if (thing_inside is not null)
            {
                error = $"You can't put {thing} in the microwave. There is already {thing_inside} inside the microwave.";
                return false;
            }
            else
            {
                error = $"I don't really feel like putting {thing} into the microwave.";
                return false;
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

        public override void Use(IPlayer sender, IThing other)
        {
            if (other is null)
            {
                if (CheckMicrowaveReady(sender))
                {
                    IThing thing_inside = this.Inventory.FirstOrDefault();
                    if (thing_inside is Tofu tofu)
                    {
                        if (tofu.Frozen)
                        {
                            tofu.Frozen = false;
                            sender.Reply($"The {tofu} immediately unfreezes.");
                        }
                        else
                        {
                            sender.Reply($"The {this} seems to only unfreeze things. The {tofu} still is room temperatured.");
                        }
                    }
                    else
                    {
                        sender.Reply($"The {this} turns on for 30 seconds. Nothing really happens. You wasted some energy. Somewhere in the rain forest a tree dies. Maybe 'put' something inside.");
                    }
                }
           }
            else 
            {
                base.Use(sender, other);
            }
        }

        private bool CheckMicrowaveReady(IPlayer sender)
        {
            if (IsOpen)
            {
                sender.Reply($"The {this} is still open.");
                return false;
            }

            if (!Game.Kitchen.Powercord.Connected || Game.DiningRoom.Ergometer.CurrentlyInUseBy == null)
            {
                sender.Reply($"The {this} needs power.");
                return false;
            }
            return true;
        }

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */
    }
}
