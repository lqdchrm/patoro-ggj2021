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

        internal static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, T separator)
            where T : IEquatable<T>
            => new SpanSplitEnumerator<T>(span, separator);
        internal static SpanSplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separator)
            where T : IEquatable<T>
            => new SpanSplitEnumerator<T>(span, separator);

        internal static CommandParser.CommandParserBuilder Parser(this IList<string> list)
        {
            return new CommandParser.CommandParserBuilder(list);
        }

    }

    internal ref struct SpanSplitEnumerator<T> where T : IEquatable<T>
    {
        private readonly ReadOnlySpan<T> _buffer;

        private readonly ReadOnlySpan<T> _separators;
        private readonly T _separator;

        private readonly int _separatorLength;
        private readonly bool _splitOnSingleToken;

        private readonly bool _isInitialized;

        private int _startCurrent;
        private int _endCurrent;
        private int _startNext;

        /// <summary>
        /// Returns an enumerator that allows for iteration over the split span.
        /// </summary>
        /// <returns>Returns a <see cref="System.SpanSplitEnumerator{T}"/> that can be used to iterate over the split span.</returns>
        public SpanSplitEnumerator<T> GetEnumerator() => this;

        /// <summary>
        /// Returns the current element of the enumeration.
        /// </summary>
        /// <returns>Returns a <see cref="System.Range"/> instance that indicates the bounds of the current element withing the source span.</returns>
        public Range Current => new Range(_startCurrent, _endCurrent);

        internal SpanSplitEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separators)
        {
            _isInitialized = true;
            _buffer = span;
            _separators = separators;
            _separator = default!;
            _splitOnSingleToken = false;
            _separatorLength = _separators.Length != 0 ? _separators.Length : 1;
            _startCurrent = 0;
            _endCurrent = 0;
            _startNext = 0;
        }

        internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator)
        {
            _isInitialized = true;
            _buffer = span;
            _separator = separator;
            _separators = default;
            _splitOnSingleToken = true;
            _separatorLength = 1;
            _startCurrent = 0;
            _endCurrent = 0;
            _startNext = 0;
        }


        /// <summary>
        /// Advances the enumerator to the next element of the enumeration.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the enumeration.</returns>
        public bool MoveNext()
        {
            if (!_isInitialized || _startNext > _buffer.Length)
            {
                return false;
            }

            ReadOnlySpan<T> slice = _buffer.Slice(_startNext);
            _startCurrent = _startNext;

            int separatorIndex = _splitOnSingleToken ? slice.IndexOf(_separator) : slice.IndexOf(_separators);
            int elementLength = (separatorIndex != -1 ? separatorIndex : slice.Length);

            _endCurrent = _startCurrent + elementLength;
            _startNext = _endCurrent + _separatorLength;
            return true;
        }
    }
}
