using LostAndFound.Engine;
using LostAndFound.FindLosty._04_LivingRoom;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class MetalDoor : Thing
    {
        public override string Emoji => Emojis.Door;

        public MetalDoor(FindLostyGame game) : base(game)
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
        public bool IsOpen { get; private set; } = false;
        public bool IsDynamiteUsed { get; private set; } = false;

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string LookText =>
            (this.IsDynamiteUsed, this.IsOpen) switch
            {
                (true, false) => $"You see the dynamite burining... RUN",
                (_, true) => "Dark burn makrs are on the door. The Explosion deformed it. It can no longer close.",
                _ => $"A very study door. It would be a blast to open it."
            };

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

            if (this.IsOpen)
            {
                sender.Reply(@"
                    You kick the door, it is not so thougt anymore.".FormatMultiline());
                sender.Room.SendText($"{sender} kicks against the door.", sender);
            }
            else
            {
                sender.Reply(@"
                    You try to kick open the door, but it is much studier then you thougt.
                    The pain crawls from your feet up to you hip.
                    You try to put on a breave face.
                    ".FormatMultiline());
                sender.Room.SendText($"{sender} kicks against the door. He trys to hide the fact that he hurt himselfe.", sender);
                sender.Hit(this.Name);
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
        public override string ListenText => $"You hear scratching on the other side of the very massive door.";

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
                sender.Reply(@"
                    The door is already open.".FormatMultiline());

            }
            else
            {
                sender.Reply(@"
                    Its locked.
                    ".FormatMultiline());
                sender.Room.SendText($"{sender} pulls on {this}. But it won't open.", sender);
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
                sender.Reply(@"The door was opend with dynamite.
                                Your not realy thinking it will ever close again.".FormatMultiline());
            }
            else
            {
                sender.Reply("It is already closed.");
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
        public override void Use(IPlayer sender, IThing other)
        {
            if (other is null)
            {
                sender.Reply($"The door feels dirty now.");
            }
            else if (other is Dynamite dynamite)
            {
                UseDynamite(sender, dynamite);
            }
            else
            {
                sender.Reply($"Using {other} has no effect on a large metal door.");
            }
        }

        public async void UseDynamite(IPlayer sender, Dynamite dynamite)
        {
            if (!this.IsDynamiteUsed)
            {
                this.IsDynamiteUsed = true;
                sender.Inventory.Remove(dynamite);


                sender.Reply($"You put the {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this}. And Ignite its fuse.");
                this.Game.EntryHall.SendText($"You see that {sender} puts a stick of {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this} and Ignites it.", sender);
                await Task.Delay(TimeSpan.FromSeconds(1)); // Dramaturgische pause
                this.Game.EntryHall.SendText("You should leave the room\nRUN");

                await Task.Delay(TimeSpan.FromSeconds(5)); // fues time

                foreach (var players in this.Game.EntryHall.Players)
                    players.Hit("explosion", damage: 3);

                this.Game.EntryHall.SendText("An explosion throws everyone in the room to the ground. Your ears are ringing.");
                foreach (var room in this.Game.Rooms.Values.Except(new[] { this.Game.EntryHall }))
                    room.SendText($"You hear a loud explosion from the {this.Game.EntryHall}. Everything is jiggles.");




                this.IsOpen = true;
                await this.Game.Cellar.Show();
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

    }
}
