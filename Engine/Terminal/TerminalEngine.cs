using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private string Mode;
        private readonly List<string> commands = null;

        public TerminalEngine(string mode, IEnumerable<string> commands = null) {
            if (mode != "interactive" && mode != "script")
                throw new Exception("Illegal terminal mode: ${mode}. Should be [interactive, script]");
            
            this.Mode = mode;
            this.commands = commands != null
                ? new List<string>(
                    commands.Where(line =>
                        !string.IsNullOrWhiteSpace(line)
                        && !line.StartsWith("##")
                    ))
                : null;
        }

        public void Dispose() { }
        public Task HideRoom(TRoom room) => Task.CompletedTask;
        public Task InitRoom(TRoom room) => Task.CompletedTask;
        public void MovePlayerTo(TPlayer player, TRoom room) { var oldRoom = player.Room; player.Room = room; Game.RaisePlayerChangedRoom(player, oldRoom); }
        public string FormatThing(TThing thing) =>
            !string.IsNullOrWhiteSpace(thing.Emoji)
            ? $"[{thing.Emoji} {thing.Name}]"
            : $"[{thing.Name}]";

        public void Mute(TPlayer player) { }
        public void Unmute(TPlayer player) { }
        public void SendImageTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public void SendReplyTo(TPlayer player, string msg) 
        {
            Task.Run(async () =>
            {
                IEnumerable<string> lines = msg.Replace("\r", "").Replace("\t", "    ").Split("\n");
                var width = lines.Max(l => l.Length);
                lines = lines.Select(l => l.PadRight(width));
                var line = string.Join("", Enumerable.Range(0, width+2).Select(i => "═"));
                var first = $"╔{line}╗";
                var last = $"╚{line}╝";

                msg = $"\n{first}\n║ {string.Join(" ║\n║ ", lines)} ║\n{last}";
                Console.WriteLine(msg.Replace("\n", "\r\n"));

                if (Mode == "script")
                    await Task.Delay(50);
            }).Wait();
        }

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

        public async Task StartEngine()
        {
            // Create single player
            var player = Game.CreateAndAddPlayer("Player");

            var startRoom = Game.Rooms.Values.First(r => r.IsVisible);
            foreach(var p in Game.Players.Values)
                p.MoveTo(startRoom);

            await Task.Run(async () =>
            {
                string input;
                while ((input = GetPlayerInput(player)) != "exit")
                {
                    var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, input);
                    if (Mode == "script")
                    {
                        if (string.IsNullOrWhiteSpace(cmd.Command))
                            continue;

                        if (input == "." && commands?.Any() is not null)
                        {
                            input = commands.FirstOrDefault();
                            if (input is not null)
                            {
                                commands.RemoveAt(0);
                                cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, input);
                            }
                        }

                        if (cmd.Command == "#end")
                            Mode = "interactive";

                        Console.Error.WriteLine($"📜 {cmd.Command} {string.Join(" ", cmd.RawArgs)}");

                        if (cmd.Command.StartsWith("#delay"))
                        {
                            if (int.TryParse(cmd.First, out int waitTime))
                                await Task.Delay(waitTime);
                        }

                        if (!cmd.Command.StartsWith("#"))
                            Game.RaiseCommand(cmd);
                    } else
                    {
                        Game.RaiseCommand(cmd);
                    }
                }
            });
        }
    }
}
