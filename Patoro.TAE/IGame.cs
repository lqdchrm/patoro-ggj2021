using System;
using System.Threading.Tasks;

namespace Patoro.TAE
{
    public interface IGame : IDisposable
    {
        Task StartAsync();
    }
}
