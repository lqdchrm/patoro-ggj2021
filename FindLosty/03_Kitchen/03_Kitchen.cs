using LostAndFound.Engine;

namespace LostAndFound.FindLosty._03_Kitchen
{
    public class Kitchen : Room
    {
        public Fridge Fridge { get; init; }
        public Shelves Shelves { get; init; }
        public FirePit FirePit { get; init; }
        public Microwave Microwave { get; init; }

        public Kitchen(FindLostyGame game) : base(game)
        {
            // Create Things in room
            this.Inventory.InitialAdd(
                this.Fridge = new Fridge(game),
                this.Shelves = new Shelves(game),
                this.FirePit = new FirePit(game),
                this.Microwave = new Microwave(game)
            );
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
            return $@"
                There are {this.Shelves} at one wall and a large {this.Fridge} on the other.
                A {this.Microwave} is mounted on the wall next to the {this.Shelves}.
                In the middle of the room is a large fire {this.FirePit}.
                {this.FirePit.LookText}
                There is one [door] leading to the dining room.
                 ".FormatMultiline();
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

        /*
        #region LocalState
        bool FirePitOn = false;
        bool FridgeDoorOpen = false;
        bool ShelvesDoorOpen = false;
        string ThingInMicroWave = null;
        string ThingInMicroWaveIcon = null;
        #endregion

        #region Inventory
        protected override IEnumerable<(string, string, string)> InitialInventory =>
            new List<(string, string, string)> {
            // ("keys", Emojis.Keys, "Some Keys"),
        };
        #endregion

        #region HELP
        protected override bool IsCommandVisible(string cmd)
        {
            return base.IsCommandVisible(cmd);
        }
        #endregion

        #region LOOK
        protected override bool IsItemVisible(string itemKey)
        {
            return base.IsItemVisible(itemKey);
        }

        protected override string DescribeRoom(GameCommand cmd)
        {
            var pit_string = DescribeThing("pit", cmd);
            var roast_string = DescribeThing("roast", cmd);
            var microwave_string = DescribeThing("microwave", cmd);
            return $@"
                There are [shelves] at one wall and a large [refrigerator] on the other.
                In the middle of the room is a large fire [pit]. {pit_string}
                {roast_string}
                {microwave_string}
                There is one [door] leading to the dining room.
        ".FormatMultiline();
        }

        protected override string DescribeThing(string thing, GameCommand cmd)
        {
            switch (thing.ToLowerInvariant())
            {
                case "shelf":
                case "shelves":
                    {
                        return "Some shelves.";
                    }
                case "roast":
                    {
                        return "On the fire pit there is a tasty pork [roast]. It's well done.";
                    }
                case "fire_pit":
                case "pit":
                    {
                        if (FirePitOn)
                            return $"The fire is roaring.";
                        else
                            return $"There is no fire. Some logs are still smoldering.";
                    }
                case "refrigerator":
                    {
                        string message = "A large fridge.";
                        if (FridgeDoorOpen)
                            message += " The door is open.";
                        if (Inventory.ContainsKey("tofu"))
                            message += " There is a box of [tofu] inside.";
                        return message;
                    }
                case "microwave":
                    {
                        string message = "A [microwave].";
                        if (ThingInMicroWave == null)
                            message += " There is nothing inside.";
                        else
                            message += $" There is {ThingInMicroWave} inside.";
                        return message;
                    }
                default:
                    return base.DescribeThing(thing, cmd);
            }
        }
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return "The refrigerator hums and the fire is soaring.";
        }

        protected override string ListenAtThing(string thing, GameCommand cmd)
        {
            switch (thing)
            {
                case "refrigerator": return $"It hums.";
                case "shelf":
                case "shelves": return $"You can faintly hear 'The sound of silence' by Simon and Garfunkel.";
                default: return base.ListenAtThing(thing, cmd);
            }
        }

        #endregion

        #region TAKE
        protected override string WhyIsItemNotTakeable(string itemKey)
        {
            return base.WhyIsItemNotTakeable(itemKey);
        }
        #endregion

        #region KICK
        protected override string KickThing(string thing, GameCommand cmd)
        {
            return base.KickThing(thing, cmd);
        }

        #endregion


        #region OPEN
        protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        {
            switch(thing)
            {
                case "shelves":
                case "shelf":
                    {
                        if (!ShelvesDoorOpen)
                        {
                            Inventory.Create("matches", Emojis.Heart, "Some matches.");
                            ShelvesDoorOpen = true;
                            return (true, "The shelf door opens.");
                        }
                        else
                            return (false, "It's already open.");

                    } break;
                case "fridge":
                case "refrigerator":
                    {
                        if (!FridgeDoorOpen)
                        {
                            Inventory.Create("tofu", Emojis.Heart, "Some delicious tofu. Consumable by humans and animals.");
                            FridgeDoorOpen = true;
                            return (true, "The refrigerator door opens.");
                        }
                        else
                            return (false, "It's already open.");
                    } break;
                default:
                    return base.OpenThing(thing, cmd);
            }
        }
        #endregion

        [Command("PUT", "put [something] in [something], eg put door in microwave")]
        public void PutCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            if (cmd.Args.Count == 3 && cmd.Args[1] == "in")
            {
                if (player.Inventory.ContainsKey(cmd.Args[0]))
                {
                    if (cmd.Args[2] == "microwave")
                    {
                        if (ThingInMicroWave != null)
                        {
                            player.Reply($"There is already {ThingInMicroWave} inside");
                        }
                        else
                        {
                            player.Reply($"You put the {cmd.Args[0]} in the micowave");
                            ThingInMicroWave = cmd.Args[0];
                            ThingInMicroWaveIcon = player.Inventory[cmd.Args[0]].Emoji;
                            player.Inventory.Remove(cmd.Args[0]);
                        }
                    }
                    else
                    {
                        player.Reply($"Put it in the what?!!??");
                    }
                }
                else
                {
                    player.Reply($"You don't have a {cmd.Args[0]}");
                }

            }
            else
            {
                player.Reply($"You need to 'put <something> in <something>'");
            }
        }

        [Command("GET", "get [something] from [something], eg get door from microwave")]
        public void GetCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            if (cmd.Args.Count == 3 && cmd.Args[1] == "from")
            {
                    if (cmd.Args[2] == "microwave")
                    {
                        if (ThingInMicroWave == cmd.Args[0])
                        {
                            ThingInMicroWave = null;
                            player.Inventory.Create(cmd.Args[0], ThingInMicroWaveIcon, $"It's a {cmd.Args[0]}");
                            player.Reply($"You took {cmd.Args[0]}.");
                        }
                        else
                        {
                            player.Reply($"There is no {cmd.Args[0]} in the microwave.");
                        }
                    }
                    else
                    {
                        player.Reply($"I don't see a {cmd.Args[2]}");
                    }

            }
            else
            {
                player.Reply($"You need to 'get <something> from <something>'");
            }
        }
        */
    }
}

