using Patoro.TAE.Discord;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindLosty
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            if (args.Any())
            {
                var mode = args[0];
                switch (mode)
                {
                    case "interactive":
                    case "script":
                        {
                            var game = FindLostyGame.Terminal(args.FirstOrDefault(), args.ElementAtOrDefault(1));
                            await game.StartAsync();
                        }
                        break;
                    case "web":
                        {
                            //var game = FindLostyGame.Webserver(args.ElementAtOrDefault(1));
                            //await game.StartAsync();
                        }
                        break;
                }
            }
            else
            {
                var bot = new DiscordBot();
                bot.RegisterGame("FindLosty", FindLostyGame.Discord);
                await bot.Start("FindLosty");
                await Task.Delay(-1);
            }
            return 0;
        }
    }
}