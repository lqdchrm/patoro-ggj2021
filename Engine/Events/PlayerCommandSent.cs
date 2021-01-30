using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerCommandSentEvent<TGame, TPlayer>
        where TGame : DiscordGame<TGame, TPlayer>
        where TPlayer : BasePlayer<TGame, TPlayer>
    {
        public TPlayer Player { get; internal set; }
        public string Command { get; internal set; }
    }
}
