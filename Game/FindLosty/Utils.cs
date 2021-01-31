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
    }
}
