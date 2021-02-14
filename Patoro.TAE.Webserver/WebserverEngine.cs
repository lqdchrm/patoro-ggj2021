using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Patoro.TAE;
using Patoro.TAE.Events;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Patoro.TAE.Webserver
{
    public class WebserverEngine<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseEngine<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        private bool disposedValue;

        public BaseGame<TGame, TPlayer, TRoom, TContainer, TThing> Game { get; set; }

        public WebserverEngine(string port) { }

        public string FormatThing(TThing thing) =>
            !string.IsNullOrWhiteSpace(thing.Emoji)
            ? $"[{thing.Emoji} {thing.Name}]"
            : $"[{thing.Name}]";

        public Task HideRoom(TRoom room) => Task.CompletedTask;
        public Task InitRoom(TRoom room) => Task.CompletedTask;
        public void MovePlayerTo(TPlayer player, TRoom room)
        {
            var oldRoom = player.Room;
            player.Room = room;
            Game.RaisePlayerChangedRoom(player, oldRoom);
        }

        public void Mute(TPlayer player) { }
        public void Unmute(TPlayer player) { }

        public Task PrepareEngine() => Task.CompletedTask;
        public void SendImageTo(TPlayer player, string msg) => SendText(player, msg, false);
        public void SendReplyTo(TPlayer player, string msg) => SendText(player, msg, true);

        private void SendText(TPlayer player, string msg, bool doHighlight)
        {
            if (NameToIdMap.TryGetValue(player.NormalizedName, out string id))
            {
                if (ClientsById.TryGetValue(id, out (IClientProxy, TPlayer) client))
                {
                    client.Item1.SendAsync("msg", HtmlTabled(msg.Boxed(), doHighlight));
                }
            }
        }

        public void SendSpeechTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public Task ShowRoom(TRoom room) => Task.CompletedTask;

        public TRoom StartRoom { get; private set; }
        public readonly Dictionary<string, (IClientProxy, TPlayer)> ClientsById = new Dictionary<string, (IClientProxy, TPlayer)>();
        public readonly Dictionary<string, string> NameToIdMap = new Dictionary<string, string>();

        public Task StartEngine()
        {
            StartRoom = Game.Rooms.Values.First(r => r.IsVisible);

            return Task.Run(() =>
            {
                string[] args = new string[] { };
                CreateHostBuilder(args).Build().Run();
            });
        }

        private IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddSingleton(this);
                });
                webBuilder.UseStartup<Startup<TGame, TPlayer, TRoom, TContainer, TThing>>();
            });

        #region IDisposeable
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~WebserverEngine()
        // {
        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
        #endregion

        private static IEnumerable<string> HtmlTableRowed(string line, bool doHighlight)
        {
            bool isHighlight = false;
            for (int i = 0; i < line.Length; ++i)
            {
                char c = line[i];
                if (i > 0 && line[i-1] == '[') isHighlight = true;
                if (c == ']') isHighlight = false;

                var clss = doHighlight && isHighlight ? " class=\"item\"" : "";

                if (char.IsHighSurrogate(c))
                {
                    yield return $"<td{clss} colspan=\"2\">{char.ConvertFromUtf32(char.ConvertToUtf32(line, i++))}</td>";
                }
                else
                {
                    yield return $"<td{clss}>{c}</td>";
                }
            }
        }

        public static string HtmlTabled(string input, bool doHighlight)
        {
            IEnumerable<string> lines = input.Replace("\r", "").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Any())
            {
                var head = $"<table><tbody>";
                var foot = $"</tbody></table>";
                return $"{head}\n{string.Join("\n", lines.Select(line => "<tr>" + string.Join("", HtmlTableRowed(line, doHighlight)) + "</tr>"))}\n{foot}";
            } else
            {
                return "";
            }
        }
    }
}