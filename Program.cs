using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using LostAndFound.Game;

namespace LostAndFound
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var engin = new Engine.MainManagement();
            await engin.Start();
            //var game = new LostAndFoundGame();
            //await game.StartAsync();
            await Task.Delay(-1);
        }
    }
}