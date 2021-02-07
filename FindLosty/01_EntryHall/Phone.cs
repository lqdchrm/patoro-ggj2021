using LostAndFound.Engine;
using System;
using System.Linq;

namespace LostAndFound.FindLosty._01_EntryHall
{
    public class Phone : Thing
    {
        public override string Emoji => Emojis.Phone;

        public Phone(FindLostyGame game) : base(game)
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

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Description => $"An old dark {this} with a dialplate. The decorative numbers are written on a white circle.\nThe key 6 looks very used.";

        public override void Look(IPlayer sender) {
            if (sender.OmniPotentPowerOfShelf)
                sender.Reply($"Through your sheer power of shelf. You can see the numbers: 32168, 5668678661, 666\nYou start to think this power maybe useless but fun.");
            else
                sender.Reply(Description);
        }

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public override void Kick(IPlayer sender)
        {
            sender.Reply(@$"You kick the {this}. It loudly hits the floor. You feel guilty and put it back on the table.");
            sender.Room.BroadcastMsg(@$"{sender} kicks the {this} and it lands loudly on the floor. After a moment he puts it back on the table.");
        }

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */
        public override string Noises => $"Beeeeeeeeeeeeeeeeeeeeeeeeeeeeeep.....";

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
            sender.TheThingThatAskedAQuestion = this;
            sender.Reply($"You open the phone.\nAre you an electrical engineer?");
        }

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */
        public override void TakeFrom(IPlayer sender, IContainer container) => sender.Reply("The cord is to short to take it anywhere.");

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */
        public override void Use(IPlayer sender) => sender.Reply("Did you mean 'use phone call 123456'");

        public void Use(IPlayer sender, string second)
        {
            if (!second.All(x => char.IsDigit(x)))
            {
                sender.Reply("A dialplate only has numbers.");
            }
            else
            {
                var response = second switch
                {
                    "32168" => "Hallo hier ist Rosi, was kann ich heute für dich tun?",
                    "5668678661" => "I told you many times. Stop calling me",
                    "666" => "Welcome to #@!~$§, all our lines are currently busy. Please hold the line...",
                    "123456" => "Really? That was just an example. At least you know how to use a phone.",
                    "911" => "911 what's your emergency? Missing dog? Please, this line is for emergencies only.\nThey won't help you.",
                    "110" => "911 what's your emergency? Missing dog? Please, this line is for emergencies only.\nThey won't help you.",
                    "404" => "You call the number.... Nobody answers the phone.\nWell what did you expect calling a street number?",
                    _ => "You call the number.... Nobody answers the phone."
                };
                sender.Reply(response);
            }
        }

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */
        public override void answer(IPlayer player, string yes_or_no)
        {
            if (player.TheThingThatAskedAQuestion == this)
            {
                if (yes_or_no == "yes")
                {
                    player.Reply("You use your electronics knowledge to analyse the circuitry.\nI'ts really a phone. Nothing else.\nYou put everything back to together.\nThe phone better than before.");
                }
                else
                {
                    player.Reply("There is complicated electronics inside. You try to put everything back together.\nSomehow you now have spare parts.");
                }
            }
            player.TheThingThatAskedAQuestion = null;
        }

    }
}
