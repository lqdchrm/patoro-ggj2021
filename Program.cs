using LostAndFound.Game.FindLosty;
using LostAndFound.Game.LostAndFound;
using System;
using System.Threading.Tasks;

namespace LostAndFound
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var bot = new Bot();
            
            // set arg to null for game selection
            await bot.Start(typeof(FindLostyGame));
            await Task.Delay(-1);
        }
    }
}