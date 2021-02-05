using LostAndFound.Engine;

namespace LostAndFound.FindLosty
{
    public interface IThing : BaseThing<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> { }
    public abstract class Thing : BaseThingImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing
    {
        public Thing(FindLostyGame game) : this(game, false, null) { }
        public Thing(FindLostyGame game, bool transferable, string name) : base(game, transferable, name) { }


        public override void Put(IPlayer sender, IThing other)
        {
            if (other is _04_LivingRoom.LionHead lionHead)
            {
                lionHead.HandleLionPut(sender, this);
            }
            else
            {
                base.Put(sender, other);
            }
        }
    }
}
