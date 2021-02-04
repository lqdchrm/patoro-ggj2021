using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IContainer : BaseContainer<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing { }

    public abstract class Container : BaseContainerImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer
    {
        public Container(FindLostyGame game) : this(game, null) { }
        public Container(FindLostyGame game, bool transferable, string name) : base(game, transferable, false, name) { }
        public Container(FindLostyGame game, string name) : base(game, true, false, name) { }
    }
}
