using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public abstract class AbstractGame : IDisposable
    {
        private bool disposedValue;
        public bool IsDisposed => disposedValue;

        public abstract Task StartAsync();

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            if (!disposedValue)
            {
                disposedValue = true;

                // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }
    }
    public abstract class AbstractGame<TGame, TPlayer> : AbstractGame
        where TGame : DiscordGame<TGame, TPlayer>
        where TPlayer : BasePlayer<TGame, TPlayer>
    {

        public abstract Task<TRoom> AddRoomAsync<TRoom>(TRoom room) where TRoom : Room<TGame, TPlayer>;

        public event EventHandler<PlayerChangedRoomEventArgs<TGame, TPlayer>> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(TPlayer player, Room<TGame, TPlayer> oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerChangedRoomEventArgs<TGame, TPlayer>()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<PlayerCommandSentEvent<TGame, TPlayer>> PlayerCommandSent;
        public void RaisePlayerCommand(TPlayer player, string cmd)
        {
            PlayerCommandSent?.Invoke(this, new PlayerCommandSentEvent<TGame, TPlayer>()
            {
                Player = player,
                Command = cmd
            });
        }
    }
}
