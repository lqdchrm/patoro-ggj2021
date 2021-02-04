using LostAndFound.Engine;
using LostAndFound.FindLosty._02_DiningRoom;

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

        public bool IsNapping => this.Inventory.Has(Game.Kitchen.Fridge.Tofu.Name);

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string LookText => this.CrocImage;

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

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

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

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */

        public override bool DoesItemFit(IThing thing, out string error) => base.DoesItemFit(thing, out error);

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */
        public override bool Use(IPlayer sender, IThing other, bool isFlippedCall = false)
        {
            if (other is Hamster)
            {
                sender.Room.SendText($"{sender} is trying to feed the {other} to the {this}. Shame, shame, shame.", sender);
                sender.Reply($"The {this} looks angry at you. It seems to be vegetarian. The {other} is squeeking.");
                return true;
            }
            else if (other is _03_Kitchen.Tofu tofu)
            {
                if (tofu.Frozen)
                {
                    sender.Reply(@$"
                    Carefully holding the {other} you come closer. The {this} leaps forward and with a giant snap it swallows {other}.
                    And imidetly spit it into your face.
                    It drops to the ground.
                    The hard and icy block hurts.
                    ".FormatMultiline());

                    sender.Room.SendText(@$"
                    {sender} goes near the {this} and puts the {other} in the mouth of the {this}.
                    Angry the {this} spits the block in to the face of {sender}.
                    It lands on the ground.
                    ", sender);
                    tofu.PutInto(sender, this.Game.EntryHall);
                    sender.Hit(tofu.ToString());
                }
                else
                {
                    sender.Reply(@$"
                    Carefully holding the {other} you come closer. The {this} leaps forward and with a giant snap it swallows {other}.

                    You assure your hand is still there...
                    It is.

                    The {this} leaves happily and full its guarding position.
                    It takes a napp right next to the door.
                    ".FormatMultiline());
                    sender.Room.SendText(@$"
                    {sender} goes near the {this} and puts the {other} in the mouth of the {this}.
                    The {this} leaves happily and full its guarding position.
                    It takes a napp right next to the door.
                    ", sender);
                    // TODO: STACKOVERFLOW
                    //tofu.PutInto(sender, this);
                    this.Game.DiningRoom.Show();
                }

                return true;
            }

            if (!isFlippedCall && other != null)
                return other.Use(sender, this, true);

            return !sender.Reply(this.UseText);
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
