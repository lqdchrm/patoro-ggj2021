using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerRoomChange<TGame, TPlayer, TRoom>
        where TGame : BaseGame<TGame, TPlayer, TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer, TRoom>
                where TRoom : BaseRoom<TGame, TPlayer, TRoom>

    {
        public TPlayer Player { get; internal set; }
        public TRoom OldRoom { get; internal set; }
    }
}
