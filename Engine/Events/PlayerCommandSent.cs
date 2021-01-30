using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerCommandSentEvent
    {
        public Player Player { get; internal set; }
        public string Command { get; internal set; }
    }
}
