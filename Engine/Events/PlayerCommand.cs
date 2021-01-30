using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerCommand
    {
        public string Message { get; internal set; }

        public BasePlayer Player { get; internal set; }
        public IList<BasePlayer> Mentions { get; internal set; }
        public string Command { get; internal set; }
        public IList<string> Args { get; internal set; }
    }
}
