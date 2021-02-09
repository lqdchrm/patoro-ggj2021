using System.Collections.Generic;
using System.Linq;

namespace Patoro.TAE
{
    public class BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public TGame Game => this.Sender.Game;

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
            this.Command = tokens.FirstOrDefault()?.ToLowerInvariant();
            this.First = tokens.Skip(1).FirstOrDefault()?.ToLowerInvariant();
            this.Prepo = tokens.Skip(2).FirstOrDefault()?.ToLowerInvariant();
            this.Second = tokens.Skip(3).FirstOrDefault()?.ToLowerInvariant();
            this.RawArgs = tokens.Skip(1);
        }

        public override string ToString() => $"{this.Sender}: {this.Command} {string.Join(" ", this.RawArgs)}";
    }
}
