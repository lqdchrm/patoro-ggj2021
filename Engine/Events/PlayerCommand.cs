using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerCommand<TGame, TPlayer,TRoom>
        where TGame : BaseGame<TGame, TPlayer,TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer,TRoom>
        where TRoom : BaseRoom<TGame, TPlayer, TRoom>
    {

        public TPlayer Player { get; internal set; }
        public IList<TPlayer> Mentions { get; internal set; }
        public string Command { get; internal set; }
        public IList<string> Args { get; internal set; }
    }
}
