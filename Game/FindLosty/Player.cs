using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class Player : BasePlayer
    {
        const int PLAYER_MAX_HEALTH = 5;

        private int health = PLAYER_MAX_HEALTH;
        int Health
        {
            get => health;
            set
            {
                if (User != null)
                {
                    var old = health;
                    health = value;
                    if (value == 0 && old > 0)
                    {
                        SendGameEventWithState($"You health has depleted and you are now muted. Try to heal yourself to be able to speak again (or ask the server admin to unmute you).");
                        User.ModifyAsync(x => x.Muted = true);
                    } else if (old == 0 && value > 0)
                    {
                        SendGameEventWithState($"You health was restored and you are now unmuted.");
                        User.ModifyAsync(x => x.Muted = false);
                    }
                }
            }
        }

        public readonly Inventory Inventory = new Inventory();
        public new CommonRoom Room => (base.Room as CommonRoom);

        public Player(string name, FindLostyGame game) : base(game, name) { }

        public override string ToString()
        {
            var health = string.Join("", Enumerable.Repeat(Emojis.Heart, Health));
            var items = string.Join("", Inventory.Values);
            return $"{Emojis.Player} {Name} {health} {items}";
        }

        public void SendGameEventWithState(string msg)
        {
            msg = $"```css\n{msg}\nYour Status: {this}```";
            Channel?.SendMessageAsync(msg);
        }

        public bool Hit(string by = null, Player byPlayer = null)
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
                    byPlayer.SendGameEvent($"You hit [{this}] really hard.");
                    msg += $" by [{byPlayer}], but it was probably deserved";
                }

                SendGameEventWithState(msg);
                return true;
            } else
            {
                if (byPlayer != null)
                {
                    byPlayer.SendGameEvent("Why are you hitting dead people?");
                    byPlayer.Room.SendGameEvent($"[{byPlayer}] is hitting dead [{this}]. Give a big BOOOO.....", byPlayer);
                }
            }
            return false;
        }

        public bool Heal(string by = null, Player byPlayer = null)
        {
            if (this.Health < PLAYER_MAX_HEALTH)
            {
                this.Health++;

                var msg = "You were healed";

                if (by != null)
                {
                    msg += $" by {by}";
                } else if (byPlayer != null)
                {
                    byPlayer.SendGameEvent($"You healed [{this}].");
                    msg += $" by [{byPlayer}]. You really got some friends here.";
                }

                SendGameEventWithState(msg);
                return true;
            } else
            {
                SendGameEvent("You need no healing.");
                return false;
            }
        }
    }
}
