using LostAndFound.Engine.Discord;
using LostAndFound.FindLosty;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Any())
            {
                var game = FindLostyGame.Terminal();
                await game.StartAsync();
            }
            else
            {
                var bot = new DiscordBot();
                await bot.Start(typeof(FindLostyGame));
            }

            await Task.Delay(-1);
        }
    }
}