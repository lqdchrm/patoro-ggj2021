using Patoro.TAE;

namespace FindLosty
{
    public interface IThing : BaseThing<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> { }
    public class Thing : BaseThingImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing
    {
        public Thing(FindLostyGame game, string name = null) : base(game, name) { }

        public virtual void answer(IPlayer sender, string yes_or_no)
        {
        }
    }
}
