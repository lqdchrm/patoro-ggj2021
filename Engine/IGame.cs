using System;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface IGame : IDisposable
    {
        Task StartAsync();
    }
}
