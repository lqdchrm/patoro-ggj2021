using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public class Player : BasePlayer<FindLostyGame, Room, Player, Thing>
    {
        public Player(string name, FindLostyGame game) : base(game, name) { }

        public override string Emoji => Emojis.Player;

        public override string StatusText
        {
            get
            {
                var health = string.Join("", Enumerable.Repeat(Emojis.Heart, Health));
                var items = string.Join("", Inventory.Select(i => i.Emoji));
                return $"[{Emojis.Player}{Name}] {health} {items}";
            }
        }

        /*
        ██╗  ██╗███████╗ █████╗ ██╗  ████████╗██╗  ██╗
        ██║  ██║██╔════╝██╔══██╗██║  ╚══██╔══╝██║  ██║
        ███████║█████╗  ███████║██║     ██║   ███████║
        ██╔══██║██╔══╝  ██╔══██║██║     ██║   ██╔══██║
        ██║  ██║███████╗██║  ██║███████╗██║   ██║  ██║
        ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚═╝   ╚═╝  ╚═╝
        */
        const int PLAYER_MAX_HEALTH = 5;

        private int health = PLAYER_MAX_HEALTH;

        int Health
        {
            get => health;
            set
            {
                var old = health;
                health = value;
                if (value == 0 && old > 0)
                {
                    ReplyWithState($"You health has depleted and you are now muted. Try to heal yourself to be able to speak again (or ask the server admin to unmute you).");
                    Mute();
                }
                else if (old == 0 && value > 0)
                {
                    ReplyWithState($"You health was restored and you are now unmuted.");
                    Unmute();
                }
            }
        }

        /*
        ██╗  ██╗██╗████████╗
        ██║  ██║██║╚══██╔══╝
        ███████║██║   ██║   
        ██╔══██║██║   ██║   
        ██║  ██║██║   ██║   
        ╚═╝  ╚═╝╚═╝   ╚═╝   
        */
        public bool HitCommand(string by = null, Player byPlayer = null)
        {
            if (this.Health > 0)
            {
                this.Health--;

                var msg = "You were hit";

                if (by != null)
                {
                    msg += $" by {by}";
                }
                else if (byPlayer != null)
                {
                    byPlayer.Reply($"You hit [{this}] really hard.");
                    msg += $" by [{byPlayer}], but it was probably deserved";
                }

                ReplyWithState(msg);
                return true;
            }
            else
            {
                if (byPlayer != null)
                {
                    byPlayer.Reply("Why are you hitting dead people?");
                    byPlayer.Room.SendText($"[{byPlayer}] is hitting dead [{this}]. Give a big BOOOO.....", byPlayer);
                }
            }
            return false;
        }

        /*
        ██╗  ██╗███████╗ █████╗ ██╗     
        ██║  ██║██╔════╝██╔══██╗██║     
        ███████║█████╗  ███████║██║     
        ██╔══██║██╔══╝  ██╔══██║██║     
        ██║  ██║███████╗██║  ██║███████╗
        ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝
        */
        public bool HealCommand(string by = null, Player byPlayer = null)
        {
            if (this.Health < PLAYER_MAX_HEALTH)
            {
                this.Health++;

                var msg = "You were healed";

                if (by != null)
                {
                    msg += $" by {by}";
                }
                else if (byPlayer != null)
                {
                    byPlayer.Reply($"You healed [{this}].");
                    msg += $" by [{byPlayer}]. You really got some friends here.";
                }

                ReplyWithState(msg);
                return true;
            }
            else
            {
                Reply("You need no healing.");
                return false;
            }
        }
    }
}
