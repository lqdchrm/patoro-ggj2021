using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BaseCommand<TGame, TRoom, TPlayer, TThing>
        where TGame: BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom: BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer: BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        public TGame Game => Sender.Game;
        
        public TPlayer Sender { get; init; }

        /// <summary>
        /// lowercase
        /// </summary>
        public string Command { get; init; }

        /// <summary>
        /// lowercase
        /// </summary>
        public string First { get; init; }

        /// <summary>
        /// lowercase
        /// </summary>
        public string Prepo { get; init; }

        /// <summary>
        /// lowercase
        /// </summary>
        public string Second { get; init; }

        public IEnumerable<string> RawArgs { get; init; }

        public BaseCommand(TPlayer sender, string msg)
        {
            this.Sender = sender;

            var tokens = msg.Split(" ");
            Command = tokens.FirstOrDefault()?.ToLowerInvariant();
            First = tokens.Skip(1).FirstOrDefault()?.ToLowerInvariant();
            Prepo = tokens.Skip(2).FirstOrDefault()?.ToLowerInvariant();
            Second = tokens.Skip(3).FirstOrDefault()?.ToLowerInvariant();
            RawArgs = tokens.Skip(1);
        }

        public override string ToString()
        {
            return $"{Sender}: {Command} {string.Join(" ", RawArgs)}";
        }
    }
}
