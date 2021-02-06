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
        public override string Description =>
            (this.IsDynamiteUsed, this.IsOpen) switch
            {
                (true, false) => $"You see the dynamite burning... RUN",
                (_, true) => "Dark burn marks are on the door. The explosion deformed it. It can no longer close.",
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
                sender.Reply(@"You kick the door, it is not so tough anymore.".FormatMultiline());
                sender.Room.BroadcastMsg($"{sender} kicks against the door.", sender);
            }
            else
            {
                sender.Hit();

                sender.ReplyWithState(@"
                    You try to kick open the door, but it is much sturdier then you thought.
                    The pain crawls from your feet up to you hip. You try to put on a breave face.
                    ".FormatMultiline());

                sender.Room.BroadcastMsg($"{sender} kicks against the door. He trys to hide the fact that he hurt himself.", sender);
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
        public override string Noises => $"You hear scratching on the other side of the very massive door.";

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
                sender.Reply(@"The door is already open.".FormatMultiline());
            }
            else
            {
                sender.Reply(@"Its locked.".FormatMultiline());
                sender.Room.BroadcastMsg($"{sender} pulls on {this}. But it won't open.", sender);
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
                sender.Reply(@"
                    The door was opened with dynamite.
                    You don't think it will ever close again.".FormatMultiline()
                );
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
        public override void Use(IPlayer sender) => sender.Reply($"The door feels used and dirty now.");

        public override void Use(IPlayer sender, IThing other)
        {
            Action action = other switch
            {
                Dynamite dynamite => () => UseDynamite(sender, dynamite),
                _ => () => sender.Reply($"Using {other} has no effect on a large metal door.")
            };
            action();
        }

        public async void UseDynamite(IPlayer sender, Dynamite dynamite)
        {
            if (!this.IsDynamiteUsed)
            {
                this.IsDynamiteUsed = true;
                sender.Remove(dynamite);

                sender.Reply($"You put the {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this}  and ignite its fuse.");
                this.Game.EntryHall.BroadcastMsg($"You see that {sender} puts a stick of {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this} and ignites it.", sender);

                // Dramaturgische pause
                await Task.Delay(TimeSpan.FromSeconds(1));
                
                this.Game.EntryHall.BroadcastMsg("You should leave the room NOW!!!\nRUN!!!");
                
                // fuse time
                await Task.Delay(TimeSpan.FromSeconds(5));

                foreach (var players in this.Game.EntryHall.Players)
                    players.Hit(damage: 3);

                this.Game.EntryHall.BroadcastMsg("An explosion throws everyone in the room to the ground. Your ears are ringing.");
                
                foreach (var room in this.Game.Rooms.Values.Except(new[] { this.Game.EntryHall }))
                    room.BroadcastMsg($"You hear a loud explosion from the {this.Game.EntryHall}. Everything is jiggles.");

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
