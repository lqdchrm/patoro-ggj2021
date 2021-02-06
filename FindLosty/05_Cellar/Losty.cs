using LostAndFound.Engine;
using LostAndFound.FindLosty;

public class Losty : Item
{
    public override string Emoji => Emojis.Dog;

    public Losty(FindLostyGame game) : base(game)
    {
    }

    public override string Description => @"
    Wuff!             _                
                     | \               
           _         |  \              
          / \_______/    \             
         |   |       @    \            
          \_/              \           
           \-------         \          
            _ / / /          \__         
            \| | | _|                 
             | | |  \                 
             \___/                     
                                       
                                       
";
}