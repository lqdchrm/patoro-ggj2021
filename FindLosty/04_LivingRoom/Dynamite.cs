using LostAndFound.Engine;
using LostAndFound.FindLosty._01_EntryHall;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class Dynamite : Item
    {
        public override string Emoji => Emojis.Dynamite;

        public Dynamite(FindLostyGame game) : base(game)
        {

        }

        public override void Use(IPlayer sender, IThing other)
        {
            if (other is MetalDoor door)
            {
                door.UseDynamite(sender, this);
            }
            else
            {
                base.Use(sender, other);
            }
        }

    }
}
