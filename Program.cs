using System;
using System.Text;
using System.Threading.Tasks;
using LostAndFound.Engine.Discord;
using LostAndFound.FindLosty;

namespace LostAndFound
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var bot = new DiscordBot();
            
            // set arg to null for game selection
            await bot.Start(typeof(FindLostyGame));
            await Task.Delay(-1);
        }
    }
}