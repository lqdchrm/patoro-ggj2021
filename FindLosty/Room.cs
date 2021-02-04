using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IRoom : BaseRoom<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer { }

    public abstract class Room : BaseRoomImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IRoom
    {
        public Room(FindLostyGame game) : this(game, null) { }
        public Room(FindLostyGame game, string name) : base(game, name) { }
    }
}
