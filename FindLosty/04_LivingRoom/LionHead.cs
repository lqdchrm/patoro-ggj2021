using LostAndFound.Engine;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class LionHead : Container
    {
        public override string Emoji => Emojis.Lion;

        public LionHead(FindLostyGame game) : base(game)
        {
        }

        /*
        ███████╗████████╗ █████╗ ████████╗███████╗
        ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
        ███████╗   ██║   ███████║   ██║   █████╗  
        ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
        ███████║   ██║   ██║  ██║   ██║   ███████╗
        ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
        */
        public bool IsMouthOpen { get; private set; }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */

        public override void Look(IPlayer sender)
        {
            if (this.IsMouthOpen)
            {
                sender.Reply("The jaw of the lion is wide open, you look closely and see deep into the throat. Its warm humid breath wafts in your face.");
                sender.Room.BroadcastMsg($"You see how {sender} puts his head deep in the mouth of the {this}", sender);
            }
            else
            {
                sender.Reply("The lion looks majestic, especially from up close. But it's sleeping. You hear it snore.");
            }
        }

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public override void Kick(IPlayer sender) => sender.Reply($"You should not wake the sleeping {this}.");

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        public override string Noises => "Yes this lion is sleeping. Trust me.";

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        public override void Open(IPlayer sender)
        {
            if (this.IsMouthOpen)
            {
                sender.Reply("The mouth is already open.");
            }
            else
            {
                sender.Reply("You open the mouth of the beast. A warm humid breath blows over your face.");
                sender.Room.BroadcastMsg($"{sender} rips open the {this} mouth. You think he maybe wants to put his head inside the beast.", sender);
                this.IsMouthOpen = true;
            }
        }

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */
        public override void Close(IPlayer sender)
        {
            if (this.IsMouthOpen)
            {
                sender.Reply("You close the mouth of the beast. This smells better.");
                sender.Room.BroadcastMsg($"{sender} gently closes the {this}'s mouth.");
                this.IsMouthOpen = false;
            }
            else
            {
                sender.Reply("The mouth is already shut.");
            }
        }

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */
        public override void TakeFrom(IPlayer sender, IContainer container) =>
            sender.Reply($"The {this} is mounted onto the wall. You can't get it off.");


        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */
        public override void PutIntoThis(IPlayer sender, IThing thing)
        {
            if (!this.IsMouthOpen)
                sender.Reply($"You can't put {thing} into a closed mouth.");
            else
            {
                sender.Reply($"You push the {thing} in the mouth of the {this}, But it roles back out. Now it is a little wet.");
                sender.Room.BroadcastMsg($"{sender} push a {thing} in the mouth of the {this}, but it roles back out.", sender);
            }
        }

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */

    }
}
