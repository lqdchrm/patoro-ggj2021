using LostAndFound.Engine;
using LostAndFound.FindLosty._02_DiningRoom;
using LostAndFound.FindLosty._03_Kitchen;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Croc : Container
    {
        public override string Emoji => Emojis.Croc;

        public Croc(FindLostyGame game) : base(game, false, null)
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

        public bool IsNapping => this.Inventory.Has(this.Game.Kitchen.Fridge.Tofu);

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string LookText => this.IsNapping
            ? this.CrocSleepImage
            : this.CrocImage;

        public string CrocSleepImage => @$"
The croc is nappint next to the door.
You only see its tail.

        _______________________
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |                 | |
        | |_____            | |
        | |_____.-'         | |
        | |                 | |
        | |                 | |
        | |                 | |
";

        public string CrocImage => @$"
        The {this} looks hungry. But luckily it is chained to a table.

              _____________________
             /                    /|
            /                    / |
           /____________________/ / 
          |_____________________|/| 
           || ||              || || 
           || ||              || || 
           || ||              || || 
          _||                 ||    
         | ||                 ||    
         |
         |
         |               .-._   _ _ _ _ _ _ _ _  
         |     .-''-.__.-'00 /'-' ' ' ' ' ' ' ' '-.
         |    '.___ '    .  /.--_'-' '-' '-' _'-' '._
         |     V: V 'vv-' / '_   '.       .'  _..' '.'.
          \     '=.____.=/.--'   :_.__.__:_   '.   : :
           \____________/(((____.-'        '-.  /   : :
                                        (((-'\ .' /
                                      _____..'  .'
                                   '-._____.-'
        ";



        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public override string KickText => OneOf(
            $"No! Just NO!",
            $"You take to some steps back for a nice little run-up and start sprinting...But being scared you stop some inches before reaching the {this}",
            $"You'd rather not!"
        );

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        public override string ListenText => this.IsNapping
            ? "You don't want to get that near..."
            : $"You put your Ear on the {this} and listen to its heart beat.";

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */
        public override string OpenText => "You think it will open its mouth on its own, if it want to.";
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
        public override string UseText => $"You better not touch the {this}.";

        public override void Use(IPlayer sender, IThing other)
        {
            if (other is null)
            {
                sender.Reply(this.UseText);
            }
            else
            {
                sender.Reply($"Maybe you should 'give' the {other} to the croc");
            }
        }

        public void RefuseToEatHamster(IPlayer sender, Hamster hamster)
        {
            sender.Room.SendText($"{sender} is trying to feed the {hamster} to the {this}. Shame, shame, shame.", sender);
            sender.Reply($"The {this} looks angry at you. It seems to be vegetarian. The {hamster} is squeeking.");
        }

        public bool TryToEatTofu(IPlayer sender, Tofu tofu)
        {
            if (tofu.Frozen)
            {
                sender.Reply(@$"
                    Carefully holding the {tofu} you come closer. The {this} leaps forward and with a giant snap it swallows {tofu}.
                    And immediately spit it into your face.
                    It drops to the ground.
                    The hard and icy block hurts.
                    ".FormatMultiline());

                sender.Room.SendText(@$"
                    {sender} goes near the {this} and puts the {tofu} in the mouth of the {this}.
                    Angry the {this} spits the block in to the face of {sender}.
                    It lands on the ground.
                    ", sender);
                sender.Hit(tofu.ToString());
                return false;
            }
            else if (!tofu.Warm)
            {
                sender.Reply(@$"
                    Carefully holding the {tofu} you come closer. The {this} leaps forward and with a giant snap it swallows {tofu}.
                    And immediately spit it into your face.
                    It drops to the ground. The croc growls horribly. Somehow it sounds like 'only warm tofu good'.
                    ".FormatMultiline());

                sender.Room.SendText(@$"
                    {sender} goes near the {this} and puts the {tofu} in the mouth of the {this}.
                    Angry the {this} spits the block in to the face of {sender}.
                    The croc growls horribly. Somehow it sounds like 'only warm tofu good'.
                    It lands on the ground.
                    ", sender);
                sender.Hit(tofu.ToString());
                return false;
            }
            else
            {
                sender.Reply(@$"
                    Carefully holding the {tofu} you come closer. The {this} leaps forward and with a giant snap it swallows {tofu}.

                    You assure your hand is still there...
                    It is.

                    The {this} leaves happily and full its guarding position.
                    It takes a napp right next to the door.
                    ".FormatMultiline());
                sender.Room.SendText(@$"
                    {sender} goes near the {this} and puts the {tofu} in the mouth of the {this}.
                    The {this} leaves happily and full its guarding position.
                    It takes a napp right next to the door.
                    ", sender);
                _ = this.Game.LivingRoom.Show();
                return true;
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
