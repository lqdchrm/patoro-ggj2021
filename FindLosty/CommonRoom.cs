//using LostAndFound.Engine;
//using LostAndFound.Engine.Attributes;
//using LostAndFound.Engine.Events;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace LostAndFound.Game.FindLosty
//{
//    public abstract class CommonRoom : BaseRoom
//    {
//        public new FindLostyGame Game => base.Game as FindLostyGame;
//        public new IEnumerable<Player> Players => base.Players.Cast<Player>();

//        public CommonRoom()
//        {
//            foreach (var item in InitialInventory)
//            {
//                Inventory.Create(item.key, item.icon, item.desc);
//            }
//        }

//        #region Inventory
//        protected virtual IEnumerable<(string key, string icon, string desc)> InitialInventory { get; } = new List<(string, string, string)> { };

//        public Inventory Inventory { get; } = new Inventory();

//        public HashSet<string> KnownThings { get; } = new HashSet<string>();
//        #endregion

//        #region HELP
//        protected override bool IsCommandVisible(string cmd)
//        {
//            return true;
//        }

//        [Command("HELP", "Lists all available commands for this room")]
//        public void HelpCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var intro = $"You are currently at {player.Room.Name}.\n";

//            var commands = string.Join("\n", Commands
//                .Where(cmd => cmd.Name.ToLowerInvariant() != "cheat")
//                .OrderBy(cmd => cmd.Name)
//                .Select(cmd => $"{cmd.Name} - {cmd.Description}")
//            );

//            var msg = string.Join("\n", intro, commands, "");

//            player.SendTextWithState(msg);
//        }
//        #endregion


//        #region LOOK
//        private static Regex extractionRegex = new Regex(@"\[\s*(?<key>([\w\s-])+)\s*\]", RegexOptions.Compiled);
//        private string ExtractThings(string text)
//        {
//            if (text == null) return null;

//            string newText = text;
//            var matches = extractionRegex.Matches(text).OfType<Match>();

//            foreach (var item in matches.Select(x => x.Groups["key"]?.Value).Where(x => !string.IsNullOrWhiteSpace(x)))
//            {
//                var normalizedItem = item.Replace(" ", "-").ToLowerInvariant();
//                this.KnownThings.Add(normalizedItem);
//                newText = newText.Replace($"[{item}]", $"[{normalizedItem}]");
//            }
//            return newText;
//        }

//        protected virtual bool IsItemVisible(string itemKey) => true;

//        protected virtual string DescribeRoom(GameCommand cmd) => "NOT implemented";
//        protected virtual string DescribeThing(string thing, GameCommand cmd) => "NOT implemented";

//        [Command("LOOK", "Look (optional at [thing]), eg LOOK Door")]
//        public void LookCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            string msg = null;
//            string thing = cmd.Args.ElementAtOrDefault(0)?.ToLowerInvariant();

//            if (thing != null)
//            {
//                string itemText = null;
//                if (cmd.GetTextMentions().Any())
//                {
//                    var other = cmd.GetTextMentions().FirstOrDefault();
//                    var attribs = new string[] { "beautiful", "scary", "ugly", "marvelous" };
//                    player.Reply($"[{other}] looks {attribs.TakeOneRandom()}.");
//                    other.Reply($"[{player}] is staring at you.");
//                    return;
//                }
//                else if (Inventory.ContainsKey(thing))
//                {
//                    itemText = DescribeThing(thing, new GameCommand(cmd));
//                    if (itemText == null)
//                        itemText = Inventory[thing].Description;
//                }
//                else if (player.Inventory.ContainsKey(thing))
//                {
//                    itemText = player.Inventory[thing].Description;
//                }
//                else if (KnownThings.Contains(thing))
//                {
//                    itemText = DescribeThing(thing, new GameCommand(cmd));
//                    itemText = ExtractThings(itemText);
//                }

//                msg = itemText ?? "Hmm, nothing to see.";
//            }
//            else
//            {
//                var items = Inventory.Where(kvp => IsItemVisible(kvp.Key)).Select(kvp => $"[{kvp.Value}{kvp.Key}]");
//                var roomText = DescribeRoom(new GameCommand(cmd));
//                roomText = ExtractThings(roomText);

//                if (items.Any())
//                {
//                    msg = $"You can see some things: {string.Join(", ", items)}\n\n{roomText}";
//                }
//                else
//                {
//                    msg = roomText;
//                }
//            }

//            if (msg != null)
//            {
//                var at = thing != null ? $"at {thing}" : "around";

//                player.Reply(msg);
//                SendGameEvent($"[{player}] is looking {at}", player);
//            }
//        }
//        #endregion


//        #region LISTEN
//        protected virtual string MakeSounds(GameCommand cmd)
//        {
//            var texts = new string[] {
//                "... ... ...",
//                "chirp chirp ... ...",
//                "[LOSTY]: wuff wuff woooooooooo"
//            };

//            return texts.TakeOneRandom();
//        }

//        protected virtual string ListenAtThing(string thing, GameCommand cmd) => null;

//        [Command("LISTEN", "Listen (optional at [thing]), eg LISTEN door")]
//        public void ListenCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var thing = cmd.Args.FirstOrDefault()?.ToLowerInvariant();
//            if (thing != null)
//            {
//                var msg = ListenAtThing(thing, new GameCommand(cmd));
//                if (msg != null)
//                {
//                    player.Reply(msg);
//                    SendGameEvent($"[{player}] is listening at [{thing}].", player);
//                }
//                else
//                {
//                    player.Reply("You can hear nothing.");
//                    SendGameEvent($"[{player}] is listening at [{thing}].", player);
//                }
//            }
//            else
//            {
//                player.Reply(MakeSounds(new GameCommand(cmd)));
//                SendGameEvent($"[{player}] is listening.", player);
//            }
//        }
//        #endregion


//        #region TAKE
//        protected virtual string WhyIsItemNotTakeable(string itemKey) => null;

//        [Command("TAKE", "pick up or take a [thing], eg TAKE keys")]
//        public void TakeCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var itemKey = cmd.Args.FirstOrDefault();
//            if (itemKey == null)
//                return;

//            var reason = WhyIsItemNotTakeable(itemKey);
//            if (reason is null)
//            {
//                if (KnownThings.Contains(itemKey))
//                {
//                    player.Reply($"You can't take [{itemKey}].");
//                    return;
//                }

//                var item = Inventory.Transfer(itemKey, player.Inventory);
//                if (item != null)
//                {
//                    SendGameEvent($"[{player}] now owns {item}", player);
//                    player.SendTextWithState($"You now own {item}");
//                }
//                else
//                {
//                    player.Reply($"{itemKey} not found");
//                }
//            }
//            else
//            {
//                player.Reply($"{itemKey} can't be taken: {reason}");
//            }
//        }
//        #endregion


//        #region DROP
//        [Command("DROP", "a [thing], eg DROP keys")]
//        public void DropCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var itemKey = cmd.Args.FirstOrDefault();
//            if (itemKey == null)
//            {
//                player.Reply("Drop what?");
//                return;
//            }

//            var item = player.Inventory.Transfer(itemKey, Inventory);
//            if (item != null)
//            {
//                SendGameEvent($"[{player}] dropped {item}", player);
//                player.SendTextWithState($"You dropped {item}");
//            }
//            else
//            {
//                player.Reply($"You could drop this ... but you don't have [{itemKey}]");
//            }
//        }
//        #endregion


//        #region KICK
//        protected virtual string KickThing(string thing, GameCommand cmd) => null;

//        [Command("KICK", "kick [somebody] or [thing], eg KICK door")]
//        public void KickCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var playersKicked = cmd.GetTextMentions().ToList();

//            if (playersKicked.Any())
//            {
//                playersKicked.ForEach(p => p.Hit(byPlayer: player));
//            }
//            else
//            {
//                var thing = cmd.Args.FirstOrDefault()?.ToLowerInvariant();
//                if (thing != null)
//                {
//                    if (player.Inventory.ContainsKey(thing))
//                    {
//                        var msg = $"You kicked [{thing}] into the air, catch it, and put it back.";
//                        SendGameEvent($"{player} kicked [{thing}]", player);
//                        player.Reply(msg);
//                    }
//                    else
//                    {
//                        var msg = KickThing(thing, new GameCommand(cmd));
//                        if (msg != null)
//                        {
//                            player.Reply(msg);
//                            SendGameEvent($"{player} kicked [{thing}]", player);
//                        }
//                        else if (KnownThings.Contains(thing))
//                        {
//                            player.Reply($"You kicked [{thing}]. Nothing happened..");
//                            SendGameEvent($"{player} kicked [{thing}]", player);
//                        }
//                        else
//                            player.Reply($"You kicked into thin air.");
//                    }
//                }
//                else
//                {
//                    player.Reply("You kicked into thin air.");
//                }
//            }
//        }
//        #endregion

//        #region HEAL
//        [Command("HEAL", "heal (optional [somebody]), eg HEAL loki")]
//        public void HealCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var msg = "You want to heal whom?";

//            if (cmd.Args.Count == 0)
//            {
//                if (player.Heal())
//                    SendGameEvent($"[{player}] self-healed.", player);
//            }
//            else
//            {
//                var other = cmd.GetTextMentions().FirstOrDefault();
//                if (other != null)
//                {
//                    if (other.Heal(byPlayer: player))
//                        SendGameEvent($"[{other}] was healed by [{player}]", other, player);
//                }
//                else
//                {
//                    player.Reply(msg);
//                }
//            }
//        }
//        #endregion

//        #region OPEN
//        protected virtual (bool succes, string msg) OpenThing(string thing, GameCommand cmd) => (false, "You want to open what?");
//        [Command("OPEN", "open a [thing], eg OPEN door")]
//        public void OpenCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            var thing = cmd.Args.FirstOrDefault()?.ToLowerInvariant();
//            if (thing != null)
//            {
//                var (success, msg) = OpenThing(thing, new GameCommand(cmd));
//                msg = ExtractThings(msg);

//                if (success)
//                {
//                    SendGameEvent($"[{player}] opened [{thing}]", player);
//                    SendGameEvent(msg);
//                }
//                else
//                {
//                    player.Reply(msg);
//                    SendGameEvent($"[{player}] failed to open {thing}", player);
//                }
//            }
//            else
//            {
//                player.Reply("You want to open what?");
//                SendGameEvent($"[{player}] failed to open {thing}", player);
//            }
//        }

//        [Command("CHEAT", "super secret command")]
//        public void CheatCommand(PlayerCommand cmd)
//        {
//            if (cmd.Player is not Player player) return;

//            if (cmd.Args.Count == 2 && cmd.Args[0] == "open")
//            {
//                switch (cmd.Args[1])
//                {
//                    case "00":
//                        {
//                        Game.FrontYard.Show();
//                        } break;
//                    case "01":
//                        {
//                        Game.EntryHall.Show();
//                        } break;
//                    case "02":
//                        {
//                        Game.DiningRoom.Show();
//                        } break;
//                    case "03":
//                        {
//                        Game.Kitchen.Show();
//                        } break;
//                    case "04":
//                        {
//                        Game.LivingRoom.Show();
//                        } break;
//                    case "05":
//                        {
//                        Game.Cellar.Show();
//                        } break;
//                    default:
//                        {
//                        } break;
//                }
//                SendGameEvent($"[{player}] opened {cmd.Args[1]}", player);
//            }
//        }
//        #endregion
//    }
//}
