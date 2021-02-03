using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerRoomChange<TGame, TRoom, TPlayer, TThing>
        where TGame: BaseGame<TGame, TRoom, TPlayer, TThing>
        where TPlayer: BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TRoom: BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        public TPlayer Player { get; internal set; }
        public TRoom OldRoom { get; internal set; }

        public override string ToString()
        {
            return $"{Player}: moved from {OldRoom} to {Player.Room}";
        }
    }
}
