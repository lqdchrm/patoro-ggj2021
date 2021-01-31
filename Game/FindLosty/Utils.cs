using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public static class Utils
    {
        private static Random rng = new Random();

        public static T TakeOneRandom<T>(this IEnumerable<T> list)
        {
            var idx = rng.Next(0, list.Count());
            return list.ElementAt(idx);
        }

        public static string FormatMultiline(this string text)
        {
            var lines = text.Split(new string[] { "\n\r", "\n", "\r" }, StringSplitOptions.TrimEntries).ToList();
            if (lines.Count > 0 && lines[0].Length == 0)
                lines = lines.Skip(1).ToList();

            return string.Join("\n", lines);
        }

        public static IList<Player> GetTextMentions(this Engine.Events.PlayerCommand inner)
        {
            if (inner is null)
                return Array.Empty<Player>();

            var argumentMentioneds = inner?.Player.Game.Players.Values.Cast<Player>().Where(IsPlayerNameInText) ?? Array.Empty<Player>();

            bool IsPlayerNameInText(Player player)
            {
                var splitName = player.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splitName.Length == 0)
                    return false;
                var args = inner?.Args;
                if (args is null)
                    return false;

                var indexList = args.Select((value, index) => (value, index)).ToArray() ?? (Array.Empty<ValueTuple<string, int>>());
                var possibleStarts = indexList.Where(x => x.value.Equals(splitName.First(), StringComparison.OrdinalIgnoreCase));

                return possibleStarts.Any(start =>
                {
                    if (splitName.Length + start.index > inner.Args.Count)
                        return false;

                    for (int i = 0; i < splitName.Length; i++)
                    {
                        if (!args[i + start.index].Equals(splitName[i], StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                    return true;
                });
            }

            return inner?.Mentions.Cast<Player>().Concat(argumentMentioneds).Distinct().ToList() ?? Array.Empty<Player>() as IList<Player>;
        }
    }
}
