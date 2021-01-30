using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public abstract class AbstractGame
    {
        public abstract Task StartAsync();

        public abstract Task<TRoom> AddRoomAsync<TRoom>(TRoom room) where TRoom : Room;

        public event EventHandler<PlayerChangedRoomEventArgs> PlayerChangedRoom;
        public void RaisePlayerChangedRoom(Player player, Room oldRoom)
        {
            PlayerChangedRoom?.Invoke(this, new PlayerChangedRoomEventArgs()
            {
                Player = player,
                OldRoom = oldRoom
            });
        }

        public event EventHandler<PlayerCommandSentEvent> PlayerCommandSent;
        public void RaisePlayerCommand(Player player, string cmd)
        {
            PlayerCommandSent?.Invoke(this, new PlayerCommandSentEvent()
            {
                Player = player,
                Command = cmd
            });
        }
    }
}
