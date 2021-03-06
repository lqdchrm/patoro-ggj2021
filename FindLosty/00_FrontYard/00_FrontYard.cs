﻿using System.Linq;
using Patoro.TAE;
using FindLosty._05_Cellar;
using System.Threading.Tasks;


namespace FindLosty._00_FrontYard
{
    public class FrontYard : Room
    {
        public Poo Poo { get; private set; }
        public Box Box { get; private set; }
        public Mansion Mansion { get; private set; }
        public Door Door { get; private set; }

        public FrontYard(FindLostyGame game) : base(game, "00")
        {
            this.Poo = new Poo(game);
            this.Box = new Box(game);
            this.Mansion = new Mansion(game);
            this.Door = new Door(game);

            this.Add(this.Poo, this.Box, this.Mansion, this.Door);
        }

        /*
          ███████╗████████╗ █████╗ ████████╗███████╗
          ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
          ███████╗   ██║   ███████║   ██║   █████╗  
          ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
          ███████║   ██║   ██║  ██║   ██║   ███████╗
          ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
        */

        /*
          ██╗      ██████╗  ██████╗ ██╗  ██╗
          ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
          ██║     ██║   ██║██║   ██║█████╔╝ 
          ██║     ██║   ██║██║   ██║██╔═██╗ 
          ███████╗╚██████╔╝╚██████╔╝██║  ██╗
          ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Image => Mansion.Fence;

        public override string Description{
            get {
                string[] description = {
                    $"You're looking at the front yard of 404 Foundleroy Road",
                    Mansion.ShortDescription(),
                    $"",
                    $"You hear barking. Could this be your missing dog {Game.Cellar.Losty}?",
                    $"He ran away two days ago, so you started a rescue mission with your best friends.",
                    $"Maybe he somehow entered the abandoned {Mansion}."
                };
                return System.String.Join('\n', description.Where(x => x != null));
            }
        }

        public override void Look(IPlayer sender)
        {
            this.Poo.WasMentioned = true;
            this.Box.WasMentioned = true;
            base.Look(sender);
        }

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
        public override string Noises => $"You hear distant barking. It definitely comes from the {this.Mansion} in front of you.";

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
