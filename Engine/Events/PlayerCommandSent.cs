using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerCommandSentEvent<TGame, TPlayer,TRoom>
        where TGame : DiscordGame<TGame, TPlayer,TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer,TRoom>
        where TRoom : Room<TGame, TPlayer, TRoom>
    {

        public TPlayer Player { get; internal set; }
        public string Command { get; internal set; }
    }
}
