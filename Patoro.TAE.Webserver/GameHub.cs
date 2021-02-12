using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patoro.TAE.Webserver
{
    public class GameHub<TGame, TPlayer, TRoom, TContainer, TThing>
        : Hub
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public WebserverEngine<TGame, TPlayer, TRoom, TContainer, TThing> Engine { get; }

        public GameHub(WebserverEngine<TGame, TPlayer, TRoom, TContainer, TThing> engine)
        {
            this.Engine = engine;
        }

        public void Connect(string name)
        {
            // Create single player
            if (!Engine.Callers.ContainsKey(name))
            {
                var player = Engine?.Game?.CreateAndAddPlayer(name);
                Engine?.Callers?.Add(name, Clients.Caller);
            } else
            {
                Engine.Callers[name] = Clients.Caller;
            }
        }

        public void Cmd(string msg)
        {
            Clients.
            var playerName = Engine?.Callers?.FirstOrDefault(kvp => kvp.Value == Clients.Caller).Key;
            if (playerName != null)
            {
                var player = Engine?.Game?.Players[playerName];
                var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(player, msg);
                Engine?.Game?.RaiseCommand(cmd);
            }
        }
    }
}
