using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IRoom : BaseRoom<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer { }

    public abstract class Room : BaseRoomImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IRoom
    {
        public Room(FindLostyGame game, string roomNumber) : this(game, roomNumber, null) { }
        public Room(FindLostyGame game, string roomNumber, string name = null) : base(game, roomNumber, name) { }
    }
}
