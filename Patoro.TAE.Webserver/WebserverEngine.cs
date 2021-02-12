using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Patoro.TAE;
using Patoro.TAE.Events;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Collections.Generic;

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
        public void SendImageTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public void SendReplyTo(TPlayer player, string msg)
        {
            if (Callers.TryGetValue(player.NormalizedName, out IClientProxy caller))
            {
                caller.SendAsync("msg", msg);
            }
        }

        public void SendSpeechTo(TPlayer player, string msg) => SendReplyTo(player, msg);
        public Task ShowRoom(TRoom room) => Task.CompletedTask;

        public TRoom StartRoom { get; private set; }
        public readonly Dictionary<string, IClientProxy> Callers = new Dictionary<string, IClientProxy>();

        public Task StartEngine()
        {
            StartRoom = Game.Rooms.Values.First(r => r.IsVisible);

            return Task.Run(() => {
                string[] args = new string[] { };
                CreateHostBuilder(args).Build().Run();
            });
        }

        private IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services => {
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
    }
}