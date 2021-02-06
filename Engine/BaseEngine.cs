using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface BaseEngine<TGame, TPlayer, TRoom, TContainer, TThing> : IDisposable
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        BaseGame<TGame, TPlayer, TRoom, TContainer, TThing> Game { get; set; }

        Task PrepareEngine();
        Task StartEngine();

        Task InitRoom(TRoom room);
        Task ShowRoom(TRoom room);
        Task HideRoom(TRoom room);

        string FormatThing(TThing thing);

        void Mute(TPlayer player);
        void Unmute(TPlayer player);

        void SendReplyTo(TPlayer player, string msg);
        void SendImageTo(TPlayer player, string msg);
        void SendSpeechTo(TPlayer player, string msg);

        void MovePlayerTo(TPlayer player, TRoom room);
    }
}
