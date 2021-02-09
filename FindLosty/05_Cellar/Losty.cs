using Patoro.TAE;


namespace FindLosty._05_Cellar
{
    public class Losty : Item
    {
        public override string Emoji => Emojis.Dog;

        public Losty(FindLostyGame game) : base(game)
        {
        }

        public override string Description => @"
    Wuff!            __               
                     ) \               
           _         )  \              
          /`\_______/    \             
         |   |       (@   \            
          \_/              \~           
           \-------         \~         
            _ / , /          \~__         
            \| ; | _|                 
             | ; ,   \                 
             \__/                     
                                       
                                       
";
        public override void Use(IPlayer sender, IThing other)
        {
            if (other is BottomlessPit pit)
            {
                pit.ThrowInLosty(sender);
            }
            else if (other is Hole hole)
            {
                hole.ThrowInLosty(sender);
            }
            else
            {
                base.Use(sender, other);
            }
        }

    }
}