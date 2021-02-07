using LostAndFound.Engine;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class RightDoor : Thing
    {
        public override string Emoji => Emojis.Door;

        public RightDoor(FindLostyGame game) : base(game)
        {
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Description => this.IsOpen
            ? DoorImage
            : this.Game.EntryHall.Croc.IsNapping
                ? this.Game.EntryHall.Croc.CrocSleepImage
                : $"The massive door made of dark wood is still in good condition.";

        public bool IsOpen { get; private set; } = false;

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
                sender.Reply($"Do not disturb the {this.Game.EntryHall.Croc}");
            else
                base.Kick(sender);
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
            if (IsOpen)
            {
                sender.Reply("You hear something breathing behinde the door.");
            } else
            {
                base.Listen(sender);
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
            if (!this.IsOpen)
            {
                sender.Reply(DoorImage);
                sender.Room.BroadcastMsg($"{sender} opens the {this}...startling.", sender);
                sender.Reply($"You open the door and look into the eyes of a hungry {this.Game.EntryHall.Croc}");
                this.IsOpen = true;
            }
            else
            {
                var crocText = this.Game.EntryHall.Croc.IsNapping
                    ? string.Empty
                    : $"\nAnd the {this.Game.EntryHall.Croc} is still staring at you.";
                sender.Reply($"The door is already open.{crocText}");
                if (!this.Game.EntryHall.Croc.IsNapping)
                {
                    sender.Reply(DoorImage);
                }
            }
        }

        public const string DoorImage = @"
        _______________________
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |            .-._ | |
        | |  .-''-.__.-'00 /| |
        | | '.___ '    .  /.| |
        | |  V: V 'vv-' / '_| |
        | |   '=.____.=/.--'| |
        | |----------/(((___| |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |";


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
            if (this.Game.EntryHall.Croc.IsNapping)
            {
                sender.Reply($"The {this.Game.EntryHall.Croc} sleeps in front of the door. You can't close it.");
            }
            else
            {
                sender.Reply($@"
                    You slam the {this} shut.
                    It is safe now. Is it?"
                    .FormatMultiline());
                sender.Room.BroadcastMsg($"{sender} slams the {this} shut.", sender);
                this.Game.DiningRoom.BroadcastMsg($"You hear a loud bang from the {this.Game.EntryHall}. Did someone slam a door?");
                this.IsOpen = false;
                this.Game.EntryHall.Croc.WasMentioned = false;
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
