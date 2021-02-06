using LostAndFound.Engine;
using System.Linq;

namespace LostAndFound.FindLosty
{
    public interface IContainer : BaseContainer<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing { }

    public abstract class Container : BaseContainerImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer
    {
        public Container(FindLostyGame game, string name = null) : base(game, name) { }
    }
}
