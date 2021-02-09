using Patoro.TAE;


namespace FindLosty._05_Cellar
{
    public class LightSwitch : Item
    {
        public override string Emoji => Emojis.Dog;

        public LightSwitch(FindLostyGame game) : base(game)
        {
        }

        public bool IsOn = false;

        public override string Description => @"A lightwitch.";
        public override void Use(IPlayer sender)
        {
            if (IsOn)
            {
                sender.Reply("You turn off the lights.\n");
                sender.Room.BroadcastMsg($"{sender} turns off the lights.", sender);
                IsOn = false;
            }
            else
            {
                sender.Reply("You turn on the lights.\n");
                sender.Room.BroadcastMsg($"{sender} turns on the lights.", sender);
                IsOn = true;
            }
        }

    }
}