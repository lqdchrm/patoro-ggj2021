using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._00_FrontYard
{
    public class FrontYard : Room
    {
        public Poo Poo { get; init; }
        public Box Box { get; init; }

        public FrontYard(FindLostyGame game) : base(game)
        {
            // Create Things in room
            Inventory.InitialAdd(
                Poo = new Poo(this.Game),
                Box = new Box(this.Game)
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
        //public override string LookText => OneOf($"It's {a} {this}", $"Nice, {a} {this}");
        //public override void Look(Player sender) => sender.Reply(LookText);

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        //public override string KickText => OneOf($"{this} was kicked.");
        //public override void Kick(Player sender) => sender.Reply(KickText);


        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        //public override string ListenText => OneOf($"Nothing to hear from {this}.");
        //public override void Listen(Player sender) => sender.Reply(ListenText);

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        //public override string OpenText => OneOf($"You can't open {this}.");
        //public override void Open(Player sender) => sender.Reply(OpenText);

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

        //public override string CloseText => OneOf($"You can't close {this}.");
        //public override void Close(Player sender) => sender.Reply(CloseText);

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */

        //public override string TakeText => OneOf($"You can't TAKE THAT™.");
        //public override void Take(Player sender) => sender.Reply(TakeText);


        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */
        //public override string PutText(Thing container)
        //{
        //    if (container is null)
        //        return OneOf($"Where do you want to put {this}");

        //    return OneOf($"You can't put {this} into {container}");
        //}

        //public override bool Put(Player sender, Thing container)
        //{
        //    sender.Reply(PutText(container));
        //    return false;
        //}

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */
        //public override string UseText(Thing other)
        //{
        //    if (other is null)
        //        return OneOf($"You can't use {Name}");

        //    return OneOf($"You can't use {other.Name} with {Name}");
        //}

        //public override bool Use(Player sender, Thing other)
        //{
        //    sender.Reply(UseText(other));
        //    return false;
        //}


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
                private bool frontDoorOpen = false;
                public bool FrontDoorOpen
                {
                    get => frontDoorOpen;
                    set
                    {
                        if (!frontDoorOpen)
                        {
                            frontDoorOpen = true;
                            Game.EntryHall.Show();
                        }
                    }
                }
                #endregion

                #region Inventory
                protected override IEnumerable<(string, string, string)> InitialInventory =>
                    new List<(string, string, string)>
                    {
                        ("poo", Emojis.Poo, "It's Mr. Hanky"),
                    };
                #endregion

                #region HELP
                protected override bool IsCommandVisible(string cmd)
                {
                    switch (cmd.ToLowerInvariant())
                    {
                        case "open": return KnownThings.Contains("door");
                        case "knock": return KnownThings.Contains("door");

                        default: return base.IsCommandVisible(cmd);
                    }
                }
                #endregion

                #region LOOK
                protected override bool IsItemVisible(string itemKey)
                {
                    return base.IsItemVisible(itemKey);
                }

                protected override string DescribeRoom(GameCommand cmd)
                {
                    var friends = Players.Where(p => p != cmd.Player);
                    var friendsNames = string.Join(", ", friends.Select(p => $"[{p}]"));
                    var friendsText = friends.Any()
                        ? $"Your friends {friendsNames} are here."
                        : "You are alone.";

                    return $@"
                        You're looking at the beautiful front yard of 404 Foundleroy Road.
                        A picket fence surrounds the [mansion] in front of you.
                        There seems to be only one way into the building. A large oak [door].
                        This looks like some kind of maniac lives here.

                        You hear barking.

                        {friendsText}
                    ".FormatMultiline();
                }

                protected override string DescribeThing(string thing, GameCommand cmd) => thing switch
                {
                    "door" => "A sturdy wooden [door] with a [plate].",
                    "plate" => "It reads: open [door]",
                    _ => null
                };
                #endregion

                #region LISTEN
                protected override string MakeSounds(GameCommand cmd)
                {
                    return "You here distant barking. It definitely comes from the mansion in front of you.";
                }

                protected override string ListenAtThing(string thing, GameCommand cmd)
                {
                    return base.ListenAtThing(thing, cmd);
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
                    string msg = null;

                    switch (thing.ToLowerInvariant())
                    {
                        case "door":
                            if (KnownThings.Contains("door"))
                            {
                                if (FrontDoorOpen)
                                {
                                    cmd.Player.Hit("swinging door");
                                    msg = "The open [door] hits the back wall and then swings back and hits your face.";
                                }
                                else
                                {
                                    cmd.Player.Hit("door");
                                    msg = "As the old saying goes: 'This will hurt you a lot more than it will the [door].' The [door] shakes. You hurt.";
                                }
                            }
                            break;
                    }
                    return msg;
                }
                #endregion

                #region OPEN
                protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
                {
                    switch (thing.ToLowerInvariant())
                    {
                        case "door":
                            if (FrontDoorOpen)
                            {
                                return (true, "You open the [door] as much as possible.");
                            }
                            else
                            {
                                FrontDoorOpen = true;
                                return (true, "The [door] swings open. Who doesn't lock their front [door]?");
                            }
                        default:
                            return (false, "Open what?");
                    }
                }
                #endregion

                #region Custom commands: KNOCK

                [Command("KNOCK", "knock on something")]
                public void KnockCommand(PlayerCommand cmd)
                {
                    if (cmd.Player is not Player player) return;

                    if (cmd.Args.Count == 0)
                    {
                        player.Reply("Knock what?");
                    }
                    else if (cmd.Args.Count > 0 && cmd.Args[0] == "door")
                    {
                        if (FrontDoorOpen)
                        {
                            player.Reply("You knock on an open [door]. Still no one answers.");
                        }
                        else
                        {
                            player.Reply("You knock on the [door]. No one answers.");
                        }
                    }
                    else
                    {
                        var other = cmd.GetTextMentions().Intersect(this.Players).FirstOrDefault();
                        player.Reply($"You knock {other?.Name ?? cmd.Args[0]} really hard.... Nothing happens.");
                        other?.Reply($"You were knocked really hard by [{player}].");
                    }
                }
                #endregion
        */

    }
}
