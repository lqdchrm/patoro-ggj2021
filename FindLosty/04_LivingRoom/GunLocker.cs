using Patoro.TAE;

using System;
using System.Linq;


namespace FindLosty._04_LivingRoom
{
    public class GunLocker : Container
    {
        public override string Emoji => Emojis.GunLocker;

        public Dynamite Dynamite { get; }

        public GunLocker(FindLostyGame game) : base(game)
        {
            this.Dynamite = new Dynamite(game);
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

        private IPlayer openedBy;
        private IPlayer[] seenWhoOpenedIt = Array.Empty<Player>();

        public void Unlock(IPlayer openingPlayer)
        {
            if (this.openedBy is not null)
                return;

            this.openedBy = openingPlayer;
            this.seenWhoOpenedIt = openingPlayer.Room.Players.ToArray();
            this.IsOpen = true;
            this.Game.LivingRoom.Add(this.Dynamite);
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
            var message = "The heavy metal locker is secured on the wall.\n";
            if (!this.IsOpen)
                message += $@"
                        There is no way to force your way in or move it.
                        A {this.Game.LivingRoom.PinPad} is mounted under the handle."
                    .FormatMultiline();
            else if (sender == this.openedBy)
                message += $@"You have opened the door.";

            else if (this.seenWhoOpenedIt.Contains(sender))
                message += $@"{this.openedBy} was able to open it.";
            else
                message += $@"Someone was able to open it.";
            sender.Reply(message);
        }

        public string ShortDescription()
        {
            string[] description = {
                $"The gun locker is " + (IsOpen ? "open" : "closed") + ".",
            };
            return System.String.Join('\n', description.Where(x => x != null));
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
            sender.Reply($"You kick against the steel. But you only succeed in hurting your toe.");
            sender.Room.BroadcastMsg($"You witness how {sender} tries to open the {this} with a kick. And fails...", sender);
        }

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
            if (this.IsOpen)
            {
                sender.Reply("The door is already open.");
            }
            else
            {
                sender.Reply("It is locked.");
                sender.Room.BroadcastMsg($"{sender} pulls on the handle of the {this}. But it does not open.", sender);
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
            if (this.IsOpen)
            {
                sender.Reply("You close the door, but it will not lock again.");
                sender.Room.BroadcastMsg($"{sender} trys to close the door of the {this}. But it swings open again.", sender);
            }
            else
            {
                sender.Reply("The door is already shut.");
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
        public override void TakeFrom(IPlayer sender, IContainer container)
        {
            sender.Reply($"Even if it was not secured on the wall. It would still be to heavy.");
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
            if (!this.IsOpen)
            {
                error = $"You can't put {thing} in {this} as long as it's closed.";
                return false;
            }
            return base.DoesItemFit(thing, out error);
        }

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


    }
}
