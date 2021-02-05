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
        public override string LookText => this.IsOpen
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
        public override string KickText => this.IsOpen
            ? $"Do not disturb the {this.Game.EntryHall.Croc}"
            : base.KickText;

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        public override string ListenText => this.IsOpen
            ? base.ListenText
            : "You hear something breatihg";

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
                sender.Room.SendText($"{sender} opens the {this}...startling.", sender);
                sender.Reply($"You open the door and look into the eyes of an hungry {this.Game.EntryHall.Croc}");
                this.IsOpen = true;
            }
            else
            {
                var crocText = this.Game.EntryHall.Croc.IsNapping
                    ? string.Empty
                    : $"\nAnd the {this.Game.EntryHall.Croc} is still staring at you.";
                sender.Reply($"The door is already opend.{crocText}");
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
                    You smash the {this}.
                    It is safe now. Is it?"
                    .FormatMultiline());
                sender.Room.SendText($"{sender} closes the {this} with a smash.", sender);
                this.Game.DiningRoom.SendText($"You hear a loud bang from the {this.Game.EntryHall}.");
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
