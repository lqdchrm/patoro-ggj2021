using LostAndFound.Engine;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Staircase : Thing
    {
        public override string Emoji => Emojis.Stairs;

        public Staircase(FindLostyGame game) : base(game)
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

        private bool isBroken;

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Description => this.isBroken
            
            ? @$"
            Like many elements in this room the {this} are made of dark wood.
                But the lower stairs are broken... you can no longer use them.
                
                Under the {this} is a concret wall that seems to cover the basement stairs.
                {A} {this.Game.EntryHall.MetalDoor} is blocking your way into the basement.".FormatMultiline()
            
            : @$"
                Like many elements in this room the stairs are made of a dark wood.
                It looks old and ... not in a good way. It could be hazardous.
                Below the {this} is {a} {this.Game.EntryHall.MetalDoor}.".FormatMultiline();

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
        public override string Noises => $"You hear barking from under the {this}.";

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
            if (this.isBroken)
            {
                sender.Reply("There is not enough of the stairs left to climb up.");
            }
            else
            {
                sender.Hit();
                sender.ReplyWithState(@$"
                        You go up the stairs, but halfway up the stairs break and with a louad squash you fell down.
                        You received damage.
                        Under the Staircase is a concreat wall that seems to cover the basement stairs.
                        A {this.Game.EntryHall.MetalDoor} is blocking the way to the basement.
                        ".FormatMultiline());

                sender.Room.BroadcastMsg(@$"
                    You see how {sender} climbs up the stairs, when suddenly the staircase collapses and {sender} forcefully hits the ground.
                    Under the {this} is a concret wall that seems to cover the basement stairs.
                    A {this.Game.EntryHall.MetalDoor} is blocking the way to the basement.
                    ".FormatMultiline(), sender);

                this.Game.DiningRoom.BroadcastMsg(@$"You hear a load noise. Something seems to be collapsed in the {this.Game.EntryHall}.");
                this.Game.LivingRoom.BroadcastMsg(@$"You hear a load noise. Something seems to be collapsed in the {this.Game.EntryHall}.");
                this.Game.Kitchen.BroadcastMsg(@$"You hear a load noise. Something seems to be collapsed in another room.");
                this.isBroken = true;
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
