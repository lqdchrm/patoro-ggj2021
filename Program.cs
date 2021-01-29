using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
namespace LostAndFound
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var game = new LostAndFoundGame();
            await game.Start();
        }

        private static async Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (string.Equals(e.Message.Content, "hello", StringComparison.OrdinalIgnoreCase))
            {
                var msg = e.Message.Author.Username;
                await e.Message.RespondAsync(msg);
            }
        }
    }
}