using Patoro.TAE;

using System;

namespace FindLosty._01_EntryHall
{
    public class LeftDoor : Thing
    {
        public override string Emoji => Emojis.Door;

        public LeftDoor(FindLostyGame game) : base(game)
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
        public bool IsOpen = false;

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Description => this.door_life == 0
            ? "Someone really did not like this door. It is shattered."
            : $"The door is made of dark wood and must have been once very beautiful.\nNow it looks like a single kick might shatter it into a thousand splinters.";

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */

        private int door_life = 3;
        private int kick_count = 0;
        private DateTimeOffset time_of_last_kick = DateTimeOffset.MaxValue;

        public override void Kick(IPlayer sender)
        {
            var time_of_kick = DateTimeOffset.Now;

            var delta = time_of_kick - this.time_of_last_kick;
            if (delta > TimeSpan.Zero && delta < TimeSpan.FromSeconds(3))
            {
                this.kick_count++;
            }
            else
            {
                this.kick_count = 1;
            }
            this.time_of_last_kick = time_of_kick;

            if (this.door_life == 0)
            {
                if (this.Game.EntryHall.Has(this.Game.EntryHall.Splinters))
                {
                    this.Game.EntryHall.Splinters.Kick(sender);
                }
                else
                {
                    sender.Reply($"There are already enough {this.Game.EntryHall.Splinters}. Someone propably has a bunch of {this.Game.EntryHall.Splinters} already.");
                }
            }
            else if (this.kick_count == 1)
            {
                sender.Reply($"The door shakes and there are some cracking sounds. But it feels like you need more force.");
            }
            else if (this.kick_count == 2)
            {
                sender.Reply($"The combined forces shake the door and there are cracking sounds. But it feels like you need more force.");
            }
            else if (this.kick_count > 2 && this.door_life > 1)
            {
                sender.Reply($"The combined forces shake the door and you can feel it crack. You definitely destroyed it a little.");
                this.door_life -= 1;
            }
            else if (this.kick_count > 2 && this.door_life == 1)
            {
                var message = $"The combined forces shatter the door into {this.Game.EntryHall.Splinters}.\nThe {this} finally opened.";
                sender.Room.BroadcastMsg(message);
                this.door_life -= 1;
                this.IsOpen = true;
                _ = this.Game.DiningRoom.Show();
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

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */
        public override void Open(IPlayer sender) => sender.Reply(
            this.IsOpen
            ? "There is not much left that could be opened."
            : $"The door is jammed. It might need united force to break it open."
        );

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */
        public override void Close(IPlayer sender) => sender.Reply(
            this.IsOpen
            ? "You don't think the remains can close anything."
            : "It's already closed."
        );

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

    }
}
