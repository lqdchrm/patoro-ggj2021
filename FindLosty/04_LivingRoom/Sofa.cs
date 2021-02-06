using System.Collections.Generic;
using System.Linq;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class Sofa : Thing
    {
        public override string Emoji => Emojis.Sofa;

        public Sofa(FindLostyGame game) : base(game)
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

        private const int NUMBER_OF_SEATS = 5;

        public IEnumerable<IPlayer> SeatedPlayers => this.Game.LivingRoom.Players.Where(x => x.ThingPlayerIsUsingAndHasToStop == this);

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string Description => (NUMBER_OF_SEATS - SeatedPlayers.Count() - 1) switch
        {

            int i when i > 1 => @$"
                The sofa is made of a dark leather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoying it. {i} seats left.
                ".FormatMultiline(),
            int i when i == 1 => @$"
                The sofa is made of a dark leather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoying it. Only one seat left.
                ".FormatMultiline(),
            int i when i == 0 => @$"
                The sofa is made of a dark leather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoying it. There is no seat left. But you think you still would find a place, when you push some.
                ".FormatMultiline(),
            int i when i == -1 => @$"
                The sofa is made of a dark leather. It would look comfortable if not to many people where already sitting on it.
                {JoinLast(this.SeatedPlayers)} are having an unconfortable time.
                ".FormatMultiline(),
            _ => $@"The sofa is made of a dark leather. It looks comfortable. {NUMBER_OF_SEATS} people can at least take place.",
        };

        private static string JoinLast<T>(IEnumerable<T> enumerable, string seperator = ", ", string lastSeperator = " and ")
        {
            return enumerable.Skip(1).Any()
                ? $"{string.Join(seperator, enumerable.SkipLast(1))}{lastSeperator}{enumerable.Last()}"
                : enumerable.FirstOrDefault()?.ToString() ?? string.Empty;
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
            if (this.SeatedPlayers.Any())
            {
                sender.Reply($@"You kick against the {this}.
                            {JoinLast(this.SeatedPlayers)} who where just enjoying their time on {this} glare at you.".FormatMultiline());

                sender.Room.BroadcastMsg($@"{sender} kicks against the {this}.
                        {JoinLast(this.SeatedPlayers)} who where just enjoying their time on {this} glare at him.".FormatMultiline(),
                        excludedPlayers: this.SeatedPlayers.Concat(new[] { sender }));

                sender.Room.BroadcastMsg($@"Your good time is interrupted by an unpleasant shake.
                                You glare at {sender}, he just kicked {this}".FormatMultiline(),
                                excludedPlayers: sender.Room.Players.Except(this.SeatedPlayers));


            }

        }

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */
        public override void Open(IPlayer sender) =>
            sender.Reply($"You could open it. But then no one could enjoy this very comfortable {this}. And it would get messy.");

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
        public override void TakeFrom(IPlayer sender, IContainer container) =>
            sender.Reply($"Yeah why not. Its not like you may want to keep your back in one piece.");

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

        public override void Use(IPlayer sender)
        {
            var freeSeets = NUMBER_OF_SEATS - this.SeatedPlayers.Count();
            if (freeSeets <= 0)
            {
                sender.Reply($"This would get tight.");
            }
            else if (freeSeets == 1)
            {
                sender.Reply($"You squish yourself into the last free spot on {this}.");

                sender.Room.BroadcastMsg($"{sender} squishes himself on the {this}. It's pretty unconfortable now.", sender.Room.Players.Except(this.SeatedPlayers));
                sender.Room.BroadcastMsg($"{sender} squishes himself on the {this}. It was already full, now it looks uncomfortable.", this.SeatedPlayers.Concat(new[] { sender }));

                sender.ThingPlayerIsUsingAndHasToStop = this;
            }
            else if (freeSeets == NUMBER_OF_SEATS)
            {
                sender.Reply($"The {this} is empty, what a shame. You will fix this now. You sit down and it is very comfortable.");
                sender.Room.BroadcastMsg($"{sender} takes place on the {this}. He seems to enjoy it.", sender);
                sender.ThingPlayerIsUsingAndHasToStop = this;

            }
            else
            {
                sender.Reply($"You take one of the free spots. It is very comfortable.");
                sender.Room.BroadcastMsg($"{sender} also sits down on the {this}. You are in good company.", sender.Room.Players.Except(this.SeatedPlayers));
                sender.Room.BroadcastMsg($"{sender} sits down on the {this}.", this.SeatedPlayers.Concat(new[] { sender }));
                sender.ThingPlayerIsUsingAndHasToStop = this;

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

    }
}