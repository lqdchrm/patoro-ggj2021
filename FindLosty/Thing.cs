using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IThing : BaseThing<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> { }
    public abstract class Thing : BaseThingImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing
    {
        public Thing(FindLostyGame game) : this(game, false, null) { }
        public Thing(FindLostyGame game, bool transferable, string name) : base(game, transferable, name) { }
    }
}
