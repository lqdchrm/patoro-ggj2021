using System;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Engine
{
    public static class Utils
    {
        private static Random rng = new Random();

        #region Object Extensions
        public static IEnumerable<T> Yield<T>(this T self)
        {
            yield return self;
        }
        #endregion

        #region IEnumerable Extensions
        public static T TakeOneRandom<T>(this IEnumerable<T> list)
        {
            var idx = rng.Next(0, list.Count());
            return list.ElementAt(idx);
        }
        #endregion

        #region String Extensions

        public static string FormatMultiline(this string text)
        {
            var lines = text.Replace("\r\n", "\n").Split('\n', StringSplitOptions.TrimEntries).ToList();
            if (lines.Count > 0 && lines[0].Length == 0)
                lines = lines.Skip(1).ToList();

            return string.Join("\n", lines);
        }

        public static int Levenshtein(this string self, string other)
        {
            string s = self.ToLowerInvariant();
            string t = other.ToLowerInvariant();

            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }
        #endregion

        #region IDictionary Extensions
        public static Dictionary<K, V> Merge<K, V, V2>(this Dictionary<K, V> self, Dictionary<K, V2> other)
            where V2 : V
        {
            var tmp = new Dictionary<K, V>();
            foreach (var key in other.Keys)
                tmp.Add(key, other[key]);

            return self.Union(tmp).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        #endregion

        //public static IList<TPlayer> GetTextMentions<TPlayer>(this Engine.Events.PlayerCommand inner)
        //{
        //    if (inner is null)
        //        return Array.Empty<TPlayer>();

        //    var argumentMentioneds = inner?.Player.Game.Players.Values.Cast<TPlayer>().Where(IsPlayerNameInText) ?? Array.Empty<TPlayer>();

        //    bool IsPlayerNameInText(TPlayer player)
        //    {
        //        var splitName = player.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //        if (splitName.Length == 0)
        //            return false;
        //        var args = inner?.Args;
        //        if (args is null)
        //            return false;

        //        var indexList = args.Select((value, index) => (value, index)).ToArray() ?? (Array.Empty<ValueTuple<string, int>>());
        //        var possibleStarts = indexList.Where(x => x.value.Equals(splitName.First(), StringComparison.OrdinalIgnoreCase));

        //        return possibleStarts.Any(start =>
        //        {
        //            if (splitName.Length + start.index > inner.Args.Count)
        //                return false;

        //            for (int i = 0; i < splitName.Length; i++)
        //            {
        //                if (!args[i + start.index].Equals(splitName[i], StringComparison.OrdinalIgnoreCase))
        //                    return false;
        //            }
        //            return true;
        //        });
        //    }

        //    return inner?.Mentions.Cast<Player>().Concat(argumentMentioneds).Distinct().ToList() ?? Array.Empty<Player>() as IList<Player>;
        //}
    }
}
