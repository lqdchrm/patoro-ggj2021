using LostAndFound.Engine;
using System.Linq;
using LostAndFound.FindLosty._02_DiningRoom;

namespace LostAndFound.FindLosty._03_Kitchen
{
    public class Microwave : Container
    {
        public override string Emoji => Emojis.Microwave;

        public Microwave(FindLostyGame game) : base(game)
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
        public override string Description {
            get {
                var intro = $"You see an old { this}. The years or use have left stains all over it. You wouldn't want to put food in there.";

                var hasCord = this.Game.Kitchen.Has(Game.Kitchen.Powercord, false);
                var cordText = hasCord ? $"\nThere's a long {this.Game.Kitchen.Powercord} dangling on the left side." : "";

                var openText = IsOpen ? "It is open." : "It is closed.";

                IThing thing_inside = this.FirstOrDefault();
                bool empty = thing_inside == null;
                var contentText = empty ? "" : $"There is a {thing_inside} inside.";

                return $"{intro}{cordText}\n{openText}\n{contentText}";
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
        public override void TakeFrom(IPlayer sender, IContainer other)
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

            IThing thing_inside = this.FirstOrDefault();
            bool empty = thing_inside == null;
            if (empty)
            {
                if (thing is Tofu)
                {
                    return true;
                }
                else if (thing is Hamster)
                {
                    return true;
                }
                else
                {
                    error = $"I don't really feel like putting {thing} into the microwave.";
                    return false;
                }
            }
            else
            {
                error = $"You can't put {thing} in the microwave. There is already {thing_inside} inside the microwave.";
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
        public override void Use(IPlayer sender)
        {
            bool microwave_door_open = IsOpen;
            bool power_connected =
                Game.Kitchen.Powercord.Connected && (Game.DiningRoom.Ergometer.CurrentlyInUseBy != null || Game.DiningRoom.Ergometer.Contains(Game.DiningRoom.Cage.Hamster));
            IThing thing_inside = this.FirstOrDefault();
            bool microwave_empty = thing_inside == null;
            if (microwave_door_open)
            {
                sender.Reply($"The {this} is still open.");
            }
            else if (!power_connected)
            {
                sender.Reply($"The {this} needs power.");
            }
            else if (microwave_empty)
            {
                sender.Reply($"The {this} turns on for 30 seconds. Nothing really happens. You wasted some energy. Somewhere in the rain forest a tree dies. Maybe 'put' something inside.");
            }
            else if (thing_inside is Tofu tofu)
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
            else if (thing_inside is Hamster hamster)
            {
                sender.Reply($"The microwave is about to turn on as the hamster roundhousekicks the door, jumps out of the microwave and runs away.");
                IsOpen = true;
                Room room = new Room[] { Game.DiningRoom, Game.Kitchen, Game.EntryHall, Game.FrontYard }.Where(x => x.IsVisible).TakeOneRandom();
                Transfer(hamster, room);
            }
        }

        public override void Use(IPlayer sender, IThing other) => 
            sender.Reply($"You should try 'put'ing something in the microwave and then just 'use'ing it without anything else.");

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
