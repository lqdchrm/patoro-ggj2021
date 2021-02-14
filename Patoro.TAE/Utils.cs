using System;
using System.Collections.Generic;
using System.Linq;

namespace Patoro.TAE
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

        public static string Normalize(this string input)
        {
            return string.Join("", input.ToLowerInvariant().Where(c => (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')));
        }

        public static string Boxed(this string input)
        {
            IEnumerable<string> lines = input.Replace("\r", "").Replace("\t", "    ").Split("\n");
            var width = lines.Max(l => l.Length);
            lines = lines.Select(l => l + new string(' ', (width - l.Length)));
            var line = string.Join("", Enumerable.Range(0, width + 2).Select(i => "═"));
            var first = $"╔{line}╗";
            var last = $"╚{line}╝";

            return $"\n{first}\n║ {string.Join(" ║\n║ ", lines)} ║\n{last}";
        }

        public static IEnumerable<string> AsCodePoints(this string s)
        {
            for (int i = 0; i < s.Length; ++i)
            {
                yield return char.ConvertFromUtf32(char.ConvertToUtf32(s, i));
                if (char.IsHighSurrogate(s, i))
                    i++;
            }
        }

        public static string FormatMultiline(this string text)
        {
            IEnumerable<string> lines = text.Replace("\r\n", "\n").Split('\n', StringSplitOptions.TrimEntries);
            if (lines.Any() && string.IsNullOrWhiteSpace(lines.First()))
                lines = lines.Skip(1);
            if (lines.Any() && string.IsNullOrWhiteSpace(lines.Last()))
                lines = lines.SkipLast(1);

            return string.Join("\n", lines.ToList());
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
    }
}
