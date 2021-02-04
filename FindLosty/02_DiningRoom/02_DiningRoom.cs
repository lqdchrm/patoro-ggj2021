using LostAndFound.Engine;

namespace LostAndFound.FindLosty._02_DiningRoom
{
    public class DiningRoom : Room
    {
        public Button Button { get; init; }
        public Cage Cage { get; init; }
        public Door Door { get; init; }
        public Ergometer Ergometer { get; init; }
        public Scanner Scanner { get; init; }
        public Table Table { get; init; }
        public Socket Socket { get; init; }

        public DiningRoom(FindLostyGame game) : base(game, "DiningRoom")
        {
            this.Button = new Button(game);
            this.Cage = new Cage(game);
            this.Door = new Door(game);
            this.Ergometer = new Ergometer(game);
            this.Scanner = new Scanner(game);
            this.Table = new Table(game);
            this.Socket = new Socket(game);

            this.Inventory.InitialAdd(this.Button, this.Cage, this.Door, this.Ergometer, this.Scanner, this.Table, this.Socket);
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
        public override string LookIntroText(IPlayer sender)
        {
            var text = @$"
                There is a {this.Table} with four chairs in one corner.
                Next to one of the chairs is a red {this.Button}.
                On the other side of the room is an {this.Ergometer}.

                A {this.Door} in the right wall leads to the kitchen.";
            if (this.Scanner.WasMentioned)
            {
                text += $" There is a {this.Scanner} next to the {this.Game.DiningRoom.Door}";
            }

            return text.FormatMultiline();
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


        //#region LocalState
        //#endregion

        //#region Inventory
        //protected override IEnumerable<(string, string, string)> InitialInventory =>
        //    new List<(string, string, string)> {
        //    };
        //#endregion

        //#region HELP
        //protected override bool IsCommandVisible(string cmd)
        //{
        //    return base.IsCommandVisible(cmd);
        //}
        //#endregion

        //#region LOOK
        //protected override bool IsItemVisible(string itemKey)
        //{
        //    return base.IsItemVisible(itemKey);
        //}

        //protected override string DescribeRoom(GameCommand cmd)
        //{
        //    return @"
        //        There is a [table] with four chairs in one corner.
        //        Next to one of the chairs is a red [button].
        //        On the other side of the room is an [ergometer].

        //        A [door] in the right wall leads to the kitchen.".FormatMultiline();
        //}

        //protected override string DescribeThing(string thing, GameCommand cmd)
        //{
        //    switch(thing)
        //    {
        //        case "door":
        //            return $"It seems to be in good shape. But there's no handle. Next to it there's a machine that looks like some kind of [scanner].";

        //        case "ergometer":
        //            return "Someone seems to like rideing a bike while having breakfast. A strange [socket] is fitted onto the side.";

        //        case "scanner":
        //            return "A green pulsing light is emitted from the [scanner] and shines on everything you hold in front of it. You probably need a barcode to use it.";

        //        case "table":
        //            return "A big table is in the center of the room. Hidden under it a [cage]";

        //        case "cage":
        //            if (Inventory.Create("hamster", Emojis.Hamster, "The hamster has some barcode printed on its belly."))
        //                return "There's a hamster. Ahhh ... you released it. Now it's running all across the room.";
        //            else
        //                return "There used to be a hamster in here.";
        //    }

        //    return base.DescribeThing(thing, cmd);
        //}
        //#endregion

        //#region LISTEN
        //protected override string MakeSounds(GameCommand cmd)
        //{
        //    return "You hear something squeak.";
        //}
        //protected override string ListenAtThing(string thing, GameCommand cmd)
        //{
        //    return base.ListenAtThing(thing, cmd);
        //}
        //#endregion

        //#region TAKE
        //protected override string WhyIsItemNotTakeable(string itemKey)
        //{
        //    return base.WhyIsItemNotTakeable(itemKey);
        //}
        //#endregion

        //#region KICK
        //protected override string KickThing(string thing, GameCommand cmd)
        //{
        //    switch(thing)
        //    {
        //        case "door":
        //            cmd.Player.Hit("door");
        //            return "You kick against the door and a painful feeling rises from your feet to the hip.";


        //    }

        //    return base.KickThing(thing, cmd);
        //}
        //#endregion


        //#region OPEN
        //protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        //{
        //    switch(thing)
        //    {
        //        case "scanner":
        //            return (false, "You can't open it without tools.");
        //    }
        //    return base.OpenThing(thing, cmd);
        //}
        //#endregion
    }
}
