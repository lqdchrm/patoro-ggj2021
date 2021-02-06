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
        public void MovePlayerTo(TPlayer player, TRoom room) { var oldRoom = player.Room; player.Room = room; Game.RaisePlayerChangedRoom(player, oldRoom); }
        public string FormatThing(TThing thing) =>
            !string.IsNullOrWhiteSpace(thing.Emoji)
            ? $"\\e[31m[{thing.Emoji} {thing.Name}]\\e[0m"
            : $"\\e[31m[{thing.Name}]\\e[0m";

        public void Mute(TPlayer player) { }
        public void Unmute(TPlayer player) { }
        public void SendImageTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public void SendReplyTo(TPlayer player, string msg) => Console.WriteLine(msg);
        public void SendSpeechTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public Task ShowRoom(TRoom room) => Task.CompletedTask;
        
        public Task PrepareEngine()
        {
            return Task.CompletedTask;
        }

        public string GetPlayerInput(TPlayer player)
        {
            Console.Write($"\n{player?.Room?.Name}: {player?.StatusText}> ");
            return Console.ReadLine();
        }

        public Task StartEngine()
        {
            // Create single player
            var player = Game.CreateAndAddPlayer("Player");

            var startRoom = Game.Rooms.Values.First(r => r.IsVisible);
            foreach(var p in Game.Players.Values)
                p.MoveTo(startRoom);

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
    }
}
