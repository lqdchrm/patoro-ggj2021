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
                (true, false) => $"You see the dynamite burning... RUN.",
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
                    You try to kick open the door, but it is much sturdier than you thought.
                    The pain goes from your feet to you hip. You try to put on a brave face.
                    ".FormatMultiline());

                sender.Room.BroadcastMsg($"{sender} kicks against the door. He tries to hide the fact that he hurt himself.", sender);
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
        public override string Noises => $"You hear scratching on the other side of the very massive metal door.";

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
                sender.Reply("It's already closed. Which is the whole problem.");
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

                sender.Reply($"You put the {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this} and ignite its fuse.");
                this.Game.EntryHall.BroadcastMsg($"You see that {sender} puts a stick of {this.Game.LivingRoom.GunLocker.Dynamite} in front of the {this} and ignites it.", sender);

                // Dramaturgische pause
                await Task.Delay(TimeSpan.FromSeconds(3));
                
                this.Game.EntryHall.BroadcastMsg("You should realy not be standing here. Leave the room NOW!!!\nRUN!!!");
                
                // fuse time
                await Task.Delay(TimeSpan.FromSeconds(10));

                
                _ = this.Game.Cellar.Show();
                foreach (Room room in this.Game.Rooms.Values)
                {
                    _ = room.Hide();
                }
                foreach (Player player in  Game.Players.Values)
                {
                    player.Reply($"An earth-shattering explosion destroys the complete {Game.FrontYard.Mansion}.\n" +
                                 $"The only thing still standing is the large metal door.\n" +
                                 $"The last thing you see, is {Game.Cellar.Losty} emerging from the rubble.\n" +
                                 $"Right behind him is a myriad of black tentacles that grab him, all your friends and you and drag you into the {Game.Cellar}.");
                    player.MoveTo(Game.Cellar);
                }
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
