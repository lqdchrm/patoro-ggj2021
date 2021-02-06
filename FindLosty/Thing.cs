using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IThing : BaseThing<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> { }
    public class Thing : BaseThingImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing
    {
        public Thing(FindLostyGame game, string name = null) : base(game, name) { }
    }
}
