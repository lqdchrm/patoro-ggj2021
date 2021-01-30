using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Events
{
    public class PlayerMessage
    {
        public BasePlayer Player { get; internal set; }
        public string Message { get; internal set; }
    }
}
