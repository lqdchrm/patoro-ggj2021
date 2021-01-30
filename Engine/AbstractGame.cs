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
    public abstract class AbstractGame<TGame, TPlayer, TRoom> : AbstractGame
        where TGame : DiscordGame<TGame, TPlayer, TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer, TRoom>
        where TRoom : Room<TGame, TPlayer, TRoom>
    {

        public abstract Task<TRoomCurrent> AddRoomAsync<TRoomCurrent>(TRoomCurrent room, DSharpPlus.Permissions allow = default, DSharpPlus.Permissions deney = default) where TRoomCurrent : TRoom;

        public event EventHandler<PlayerChangedRoomEventArgs<TGame, TPlayer, TRoom>> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(TPlayer player, TRoom oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerChangedRoomEventArgs<TGame, TPlayer, TRoom>()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<PlayerCommandSentEvent<TGame, TPlayer, TRoom>> PlayerCommandSent;
        public void RaisePlayerCommand(TPlayer player, string cmd)
        {
            PlayerCommandSent?.Invoke(this, new PlayerCommandSentEvent<TGame, TPlayer, TRoom>()
            {
                Player = player,
                Command = cmd
            });
        }
    }
}
