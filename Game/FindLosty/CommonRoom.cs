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
    public abstract class CommonRoom : BaseRoom
    {
        public new FindLostyGame Game => base.Game as FindLostyGame;
        public new IEnumerable<Player> Players => base.Players.Cast<Player>();

        public CommonRoom()
        {
            foreach(var item in InitialInventory)
            {
                Inventory.Add(item.key, item.icon);
            }
        }

        #region Inventory
        protected virtual IEnumerable<(string key, string icon)> InitialInventory { get; } = new List<(string, string)> { };

        public Inventory Inventory { get; } = new Inventory();
        #endregion

        #region HELP
        protected override bool IsCommandVisible(string cmd) => true;

        [Command("HELP", "Lists all available commands for this room")]
        public void HelpCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var intro = $"You are currently at {player.Room.Name}.\n";

            var commands = string.Join("\n", Commands
                .OrderBy(cmd => cmd.Name)
                .Select(cmd => $"{cmd.Name} - {cmd.Description}")
            );

            var msg = string.Join("\n", intro, commands, "");

            player.SendGameEventWithState(msg);
        }
        #endregion


        #region LOOK
        protected virtual bool IsItemVisible(string itemKey) => true;
        protected virtual string DescribeRoom(GameCommand cmd) => "NOT implemented";
        protected virtual string DescribeThing(string thing, GameCommand cmd) => "NOT implemented";

        [Command("LOOK", "Look (optional at [thing]), e.g. LOOK Door")]
        public void LookCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            string msg = null;
            string thing = cmd.Args.ElementAtOrDefault(0);

            if (thing != null)
            {
                string itemText = null;
                if (Inventory.ContainsKey(thing) || IsItemVisible(thing))
                {
                    itemText = DescribeThing(thing, new GameCommand(cmd));
                }

                msg = itemText ?? "Hmm, nothing to see.";
            } else
            {
                var items = Inventory.Where(kvp => IsItemVisible(kvp.Key)).Select(kvp => $"[{kvp.Value}{kvp.Key}]");
                var roomText = DescribeRoom(new GameCommand(cmd));

                if (items.Any())
                {
                    msg = $"You can see some things: {string.Join(", ", items)}\n\n{roomText}";
                } else
                {
                    msg = roomText;
                }
            }

            if (msg != null)
            {
                var at = thing != null ? $"at {thing}" : "around";

                player.SendGameEvent(msg);
                SendGameEvent($"[{player}] is looking {at}", player);
            }
        }
        #endregion


        #region LISTEN
        protected virtual string MakeSounds(GameCommand cmd)
        {
            var texts = new string[] {
                "... ... ...",
                "chirp chirp ... ...",
                "[LOSTY]: wuff wuff woooooooooo"
            };

            return texts.TakeOneRandom();
        }

        [Command("LISTEN", "Listen")]
        public void ListenCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            player.SendGameEvent(MakeSounds(new GameCommand(cmd)));
            SendGameEvent($"[{player}] is listening.", player);
        }
        #endregion


        #region TAKE
        protected virtual string WhyIsItemNotTakeable(string itemKey) => null;

        [Command("TAKE", "pick up or take a [thing], eg TAKE keys")]
        public void TakeCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var itemKey = cmd.Args.FirstOrDefault();
            if (itemKey == null)
                return;

            var reason = WhyIsItemNotTakeable(itemKey);
            if (reason is null)
            {
                var item = Inventory.Transfer(itemKey, player.Inventory);
                if (item != null)
                {
                    SendGameEvent($"[{player}] now owns {item}", player);
                    player.SendGameEventWithState($"You now own {item}");
                } else
                {
                    player.SendGameEvent($"{itemKey} not found");
                }
            } else
            {
                player.SendGameEvent($"{itemKey} can't be taken: {reason}");
            }
        }
        #endregion


        #region DROP
        [Command("DROP", "a [thing], eg DROP keys")]
        public void DropCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var itemKey = cmd.Args.FirstOrDefault();
            if (itemKey == null)
                return;

            var item = player.Inventory.Transfer(itemKey, Inventory);
            if (item != null)
            {
                SendGameEvent($"[{player}] dropped {item}", player);
                player.SendGameEventWithState($"You dropped {item}");
            }
            else
            {
                player.SendGameEvent($"You can't drop this. {itemKey} not found");
            }
        }
        #endregion


        #region KICK
        protected virtual string KickThing(string thing, GameCommand cmd) => null;

        [Command("KICK", "kick st or sb")]
        public void KickCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var playersKicked = Players
                .Where(p => p != player)
                .Where(p => cmd.Message.ToLowerInvariant().Contains(p.Name.ToLowerInvariant())).ToList();

            if (playersKicked.Any())
            {
                playersKicked.ForEach(p => p.Hit(byPlayer: player));
            }
            else
            {
                var thing = cmd.Args.FirstOrDefault();
                if (thing != null)
                {
                    if (player.Inventory.ContainsKey(thing))
                    {
                        var msg = $"You kicked [{thing}] into the air, catch it, and put it back.";
                        SendGameEvent($"{player} kicked [{thing}]", player);
                        player.SendGameEvent(msg);
                    }
                    else
                    {
                        var msg = KickThing(thing, new GameCommand(cmd));
                        if (msg != null)
                        {
                            player.SendGameEvent(msg);
                            SendGameEvent($"{player} kicked [{thing}]", player);
                        }
                        else
                            player.SendGameEvent("You kicked into thin air.");
                    }
                }
                else
                {
                    player.SendGameEvent("You kicked into thin air.");
                }
            }
        }
        #endregion

        #region HEAL
        [Command("HEAL", "heal somebody or yourself")]
        public void HealCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var msg = "You want to heal whom?";

            var other = cmd.Args.FirstOrDefault()?.ToLowerInvariant();
            if (other != null)
            {
                var otherPlayer = Players.FirstOrDefault(p => p.Name.ToLowerInvariant().Equals(other));
                if (otherPlayer != null)
                {
                    if (otherPlayer.Heal(byPlayer: player))
                        SendGameEvent($"[{otherPlayer}] was healed by [{player}]", otherPlayer, player);
                } else
                {
                    player.SendGameEvent(msg);
                }
            } else
            {
                if (player.Heal())
                    SendGameEvent($"[{player}] self-healed.", player);
            }
        }
        #endregion

        #region OPEN
        protected virtual (bool succes, string msg) OpenThing(string thing, GameCommand cmd) => (false, "You want to open what?");
        [Command("OPEN", "open sth")]
        public void OpenCommand(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var thing = cmd.Args.FirstOrDefault();
            if (thing != null)
            {
                var (success, msg) = OpenThing(thing, new GameCommand(cmd));

                if (success)
                {
                    SendGameEvent(msg);
                } else
                {
                    player.SendGameEvent(msg);
                    SendGameEvent($"[{player}] failed to open {thing}", player);
                }
            } else
            {
                player.SendGameEvent("You want to open what?");
                SendGameEvent($"[{player}] failed to open {thing}", player);
            }
        }
        #endregion
    }
}
