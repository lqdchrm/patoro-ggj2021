using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine.Cnsole
{
    public class TerminalEngine<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseEngine<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public BaseGame<TGame, TPlayer, TRoom, TContainer, TThing> Game { get; set; }

        public void Dispose() { }
        public Task HideRoom(TRoom room) => Task.CompletedTask;
        public Task InitRoom(TRoom room) => Task.CompletedTask;
        public void MovePlayerTo(TPlayer player, TRoom room) { }
        public void Mute(TPlayer player) { }
        public void Unmute(TPlayer player) { }
        public bool SendImageTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public bool SendReplyTo(TPlayer player, string msg) { Console.WriteLine(msg); return true; }
        public bool SendSpeechTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public Task ShowRoom(TRoom room) => Task.CompletedTask;
        
        public Task PrepareEngine()
        {
            // Create single player
            var player = Game.CreateAndAddPlayer("Player");

            Task.Run(() =>
            {
                string input;
                while ((input = GetPlayerInput(player)) != "exit")
                {
                    var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, input);
                    Game.RaiseCommand(cmd);
                }
            });
            return Task.CompletedTask;
        }

        public string GetPlayerInput(TPlayer player)
        {
            Console.Write($"{player?.Room?.Name}: {player?.StatusText}> ");
            return Console.ReadLine();
        }

        public Task StartEngine()
        {
            var startRoom = Game.Rooms.Values.First(r => r.IsVisible);
            foreach(var player in Game.Players.Values)
            {
                player.MoveTo(startRoom);
            }

            return Task.CompletedTask;
        }
    }
}
