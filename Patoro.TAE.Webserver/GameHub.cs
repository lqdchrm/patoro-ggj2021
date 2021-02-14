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

        public void Cmd(string msg)
        {
            (IClientProxy, TPlayer) client;
            var id = Context.ConnectionId;

            if (!Engine.ClientsById.ContainsKey(id))
            {
                // check if message was like "join as playername"
                var tokens = msg.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (tokens.Length > 1 && tokens.First().ToLowerInvariant() == "join")
                {
                    var playerName = tokens.Last();
                    TPlayer player;

                    // check if it's a new player
                    if (!Engine.NameToIdMap.TryGetValue(playerName.Normalize(), out string _id))
                    {
                        if (playerName.Length < 3)
                        {
                            Clients.Caller.SendAsync("msg", "Please choose a longer name");
                        }
                        else
                        {
                            player = Engine?.Game?.CreateAndAddPlayer(playerName);
                            Engine?.ClientsById.Add(id, (this.Clients.Caller, player));
                            Engine?.NameToIdMap.Add(player.NormalizedName, id);
                            var startRoom = Engine.Game.Rooms.Values.First(r => r.IsVisible);
                            player.MoveTo(startRoom);
                            msg = null;
                        }
                    }
                    else
                    {
                        client = Engine.ClientsById[_id];
                        client.Item1 = Clients.Caller;
                        player = client.Item2;
                        Engine.ClientsById.Remove(_id);
                        Engine.ClientsById.Add(id, client);
                        Engine.NameToIdMap[player.NormalizedName] = id;
                        msg = "look";
                    }
                }
                else
                {
                    Clients.Caller.SendAsync("msg", "Are you a new player? Please join the game with: join as [playerName]");
                }
            }

            if (!string.IsNullOrWhiteSpace(msg) && Engine.ClientsById.TryGetValue(id, out client))
            {
                var cmd = new BaseCommand<TGame, TPlayer, TRoom, TContainer, TThing>(client.Item2, msg);
                Engine?.Game?.RaiseCommand(cmd);
            }
        }
    }
}
