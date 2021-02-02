using LostAndFound.Engine;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class Dynamite : Item
    {
        public override string Emoji => Emojis.Dynamite;

        public Dynamite(FindLostyGame game) : base(game)
        {


        }
    }
}