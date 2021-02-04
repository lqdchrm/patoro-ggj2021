using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface IGame : IDisposable
    {
        Task StartAsync();
    }
}
