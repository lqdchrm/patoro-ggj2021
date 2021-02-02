using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class LivingRoom : Room
    {
        public LivingRoom(FindLostyGame game) : base(game, "LivingRoom")
        {
        }

        #region LocalState
        public const string PIN = "#39820";
        private bool gunlockerOpen;
        #endregion

        #region Inventory

        #endregion

        #region HELP
        protected override bool IsCommandVisible(string cmd)
        {
            if (cmd == "enter-pin")
            {
                return (this.KnownThings.Contains("pin-pad"));
            }
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
            return @"
                You look around in the big living room. The croc is sitting next the door. A big [sopha] stands on an bright red carpet.
                Opposite the seating area is an old [chimney]. Beneath a [lion-head], that hangs next to the [chimney] is a metal [gun-locker]."
                .FormatMultiline();
        }

        protected override string DescribeThing(string thing, GameCommand cmd)
        {
            return (thing, this.gunlockerOpen) switch
            {
                ("pin-pad", _) => "The pin pad is a 10 number pad with additional keys for # and *.",
                ("sopha", _) => "The sopha is made of a dark lather. It looks comfortable.",
                ("lion-head", _) => "The lion look majestic, epically from so close. But its sleeping. You hear it snore.",
                ("gun-locker", true) => @"A heavy metal locker. The door is opened widely.",
                ("gun-locker", false) => @"
                                A heavy metal locker. There is now way to force your way in and it is secured in the wall.
                                A [pin pad] is mounted under the handle.".FormatMultiline(),
                ("chimney", _) => @"
                        The chimney gas powered, looks like it wasn't used in some time...
                        and could need some cleaning.".FormatMultiline(),
                _ => base.DescribeThing(thing, cmd)
            };
        }
        #endregion

        #region LISTEN
        protected override string MakeSounds(GameCommand cmd)
        {
            return base.MakeSounds(cmd);
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
            return base.KickThing(thing, cmd);
        }
        #endregion


        #region OPEN
        protected override (bool succes, string msg) OpenThing(string thing, GameCommand cmd)
        {
            if (thing == "gun-locker")
            {
                if (this.gunlockerOpen)
                {
                    cmd.Player.Reply("It's already open.");
                }
                else
                {
                    cmd.Player.Reply("It is locked.");
                    this.SendGameEvent($"${cmd.Player} jiggles the [gun-locker].", cmd.Player);
                }

            }
            return base.OpenThing(thing, cmd);
        }
        #endregion


        [Command("ENTER-PIN", "Enters the pin for the weapon locker (only this room), eg ENTER-PIN 123456")]
        public void EnterPin(PlayerCommand cmd)
        {
            if (!cmd.Args.Any())
            {
                var message = string.Join("", cmd.Args);
                cmd.Player.Reply($"You enter ${message}");
                if (message == PIN)
                {
                    cmd.Player.Reply($"You hear an pleasant Bing.");
                    this.SendGameEvent($"You hear Bing [gun-locker].", cmd.Player);
                    this.gunlockerOpen = true;
                    this.Inventory.Add("dynamite", new Item("dynamite",  Emojis.Dynamite, "It will explode when lit."));
                    this.SendGameEvent($"The door of the [gun-locker] swings open and a pack of [dynamite{Emojis.Dynamite}]is rolling on the floor.");
                }
                else
                {
                    cmd.Player.Reply($"An unpleasant sound informs you that this was not the correct pin.");
                    this.SendGameEvent($"You hear an unpleasant sound from the [gun-locker].", cmd.Player);
                }
            }
        }
    }
}
