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

        public virtual Task Show() => Game.SetRoomVisibilityAsync(this, true);
        public virtual Task Hide() => Game.SetRoomVisibilityAsync(this, false);

        public Inventory Inventory { get; } = new Inventory();

        protected override bool IsCommandVisible(string cmd) => true;

        [Command("HELP", "Lists all available commands for this room")]
        public async Task HelpCommand(PlayerCommand cmd)
        {
            if (cmd.Player is Player player)
            {
                var intro = $"You are currently at {player.Room.Name}.\n";

                var commands = string.Join("\n", Commands
                    .OrderBy(cmd => cmd.Name)
                    .Select(cmd => $"{cmd.Name} - {cmd.Description}")
                );

                var msg = string.Join("\n", intro, commands, "");

                await player.SendGameEventWithStateAsync(msg);
            }
        }

        [Command("LOOK", "Look (optional at [thing]), e.g. LOOK Door")]
        public virtual async Task LookAt(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var items = Inventory.Where(kvp => IsItemVisible(kvp.Key)).Select(kvp => $"[{kvp.Value}{kvp.Key}]");
            var roomText = await Describe(cmd);

            var msg = roomText;
            if (items.Any())
            {
                msg = $"You can see some things: {string.Join(", ", items)}\n\n{roomText}";
            }

            await player.SendGameEventAsync(msg);
        }
        protected virtual async Task<string> Describe(PlayerCommand cmd) => "NOT implemented";
        protected virtual bool IsItemVisible(string itemKey) => true;


        [Command("LISTEN", "Listen")]
        public virtual async Task Listen(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var texts = new string[] {
                "... ... ...",
                "chirp chirp ... ...",
                "[LOSTY]: wuff wuff woooooooooo"
            };

            await player.SendGameEventAsync(texts.TakeOneRandom());
        }

        [Command("TAKE", "pick up or take a [thing], eg TAKE keys")]
        public virtual async Task Take(PlayerCommand cmd)
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
                    await SendGameEventAsync($"[{player}] now owns {item}", player);
                    await player.SendGameEventWithStateAsync($"You now own {item}");
                } else
                {
                    await player.SendGameEventAsync($"{itemKey} not found");
                }
            } else
            {
                await player.SendGameEventAsync($"{itemKey} can't be taken: {reason}");
            }
        }
        protected virtual string WhyIsItemNotTakeable(string itemKey) => null;


        [Command("DROP", "a [thing], eg DROP keys")]
        public virtual async Task Drop(PlayerCommand cmd)
        {
            if (cmd.Player is not Player player) return;

            var itemKey = cmd.Args.FirstOrDefault();
            if (itemKey == null)
                return;

            var item = player.Inventory.Transfer(itemKey, Inventory);
            if (item != null)
            {
                await SendGameEventAsync($"[{player}] dropped {item}", player);
                await player.SendGameEventWithStateAsync($"You dropped {item}");
            }
            else
            {
                await player.SendGameEventAsync($"You can't {itemKey} not found");
            }
        }

        [Command("HIT", "Hits an opponent")]
        public async Task HitCommand(PlayerCommand cmd)
        {
            var tasks = cmd.Mentions.Cast<Player>().Select(p => p.HitAsync(cmd.Player.Name));
            await Task.WhenAll(tasks);
        }
    }
}
