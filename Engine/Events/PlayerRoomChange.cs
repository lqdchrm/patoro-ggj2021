using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerRoomChange
    {
        public BasePlayer Player { get; internal set; }
        public BaseRoom OldRoom { get; internal set; }
    }
}
