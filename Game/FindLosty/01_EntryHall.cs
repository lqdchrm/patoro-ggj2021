using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class EntryHall : CommonRoom
    {
        public override string Name => "EntryHall";

        #region LocalState
        private bool diningRoomDoorOpen = false;
        public bool DiningRoomDoorOpen
        {
            get => diningRoomDoorOpen;
            set
            {
                if (!diningRoomDoorOpen)
                {
                    diningRoomDoorOpen = true;
                    Game.DiningRoom.Show();
                }
            }
        }
        #endregion

        #region Images
        private const string croc = @"
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

        private const string crocChained = @"
The [croc] looks hungry. But luckily it is chained to a table.

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
        #endregion

        #region Inventory
        protected override IEnumerable<(string, string)> InitialInventory =>
            new List<(string, string)> {
            // ("keys", Emojis.Keys),
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
            return $@"
                You are in a great hall. The floor has a black and white checker pattern.
                To your left is the [wardrobe] and to your right a small table with a [phone].

                In the middle of the hall, on both sides in the wall, you'll find two doors.

                Both the [left door] and the [right door] are made of a massive dark wood.

                In the back of the hall is a wide [staircase]. You can also see a [window] on the back of the room if you look past the [staircase].

                The barking gets loader.
            ".FormatMultiline();
        }

        protected override string DescribeThing(string thing, GameCommand cmd)
        {
            switch (thing.ToLowerInvariant())
            {

                case "wardrobe":
                    return $"The [wardrobe] can hold many coats and cloaks.But it is empty.";
                
                case "phone":
                    return $"An old dark [phone] with a dialplate. The decorative numbers are written on a white circle.\nThe 6 looks very used.";
                
                case "left-door":
                    return $"The massive door made of dark wood must have been once very beautiful. You see some water marks on the side where the door has swollen up a little.";
                
                case "right-door":
                    return $"The massive door made of dark wood is still in good condition.";
                
                case "staircase":
                    return @$"Like many elements in this room the stairs are made of a dark wood.
                        But it looks old and ... not in a good way. It could be a hazard.
                        In the side of the staircase is a [metal door].".FormatMultiline();

                case "window":
                    return $"You look into the garden at the back of the house. It could need some care...";

                case "metal-door":
                    return @$"A very study door. It would be a blast to open it.";

                case "croc":
                    return crocChained;

                default:
                    return base.DescribeThing(thing, cmd);
            }
        }
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return "You hear barking from the back of the room.";
        }

        protected override string ListenAtThing(string thing, GameCommand cmd)
        {
            switch (thing)
            {
                case "metal-door": return $"You hear scratching on the other side of the very massive door.";
                case "phone": return $"Beeeeeeeeeeeeeeeeeeeeeeeeeeeeeep.....";
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
            switch(thing)
            {
                case "left-door": return KickDiningRoomDoor(cmd);
            }
            return base.KickThing(thing, cmd);
        }

        private int door_life = 3;
        private int kick_count = 0;
        private DateTimeOffset time_of_last_kick = DateTimeOffset.MaxValue;
        private string KickDiningRoomDoor(GameCommand cmd)
        {
            var message = "Nothing happened.";
            var time_of_kick = DateTimeOffset.Now;

            var delta = time_of_kick - time_of_last_kick;
            if (delta > TimeSpan.Zero && delta < TimeSpan.FromSeconds(3))
            {
                kick_count++;
            } else
            {
                kick_count = 1;
            }
            time_of_last_kick = time_of_kick;

            if (door_life == 0)
            {
                message = "You kick the splinters on the floor. Nothing happens.";
            }
            else if (kick_count == 1)
            {
                message = "The door shakes and there are some cracking sounds. But it feels like you need more force.";
            }
            else if (kick_count == 2)
            {
                message = "The combined force shake the door and there are cracking sounds. But it feels like you need more force.";
            }
            else if (kick_count > 2 && door_life > 1)
            {
                message = "The combined force shake the door and you can feel it crack. You definitely destroyed it a little.";
                door_life -= 1;
            }
            else if (kick_count > 2 && door_life == 1)
            {
                message = "The combined forces shatter the door into splinters.";
                cmd.Player.Room.SendGameEvent(message, cmd.Player);
                door_life -= 1;
                DiningRoomDoorOpen = true;
            }

            return message;
        }
        #endregion


        #region OPEN
        protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        {
            switch(thing)
            {
                case "left-door":
                    return (false, "The door is jammed. It might need united force to break open");

                case "right-door":
                    cmd.Player.SendGameEvent(croc, true);
                    cmd.Player.Room.SendGameEvent($"[{cmd.Player}] opens the right door...startling.", cmd.Player);
                    return (true, $"You open the door.\nAnd look in the eyes of an An hungry [croc].");

                case "croc":
                    return (false, "No! Just NO!");
            }

            return base.OpenThing(thing, cmd);
        }
        #endregion
    }
}
