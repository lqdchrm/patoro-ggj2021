using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;

namespace LostAndFound
{

    public class LostAndFoundGame : Game
    {
        protected override async Task InitGame()
        {
            await CreateRoomAsync("Taverne");
            await CreateRoomAsync("Wald");
        }
    }
}
