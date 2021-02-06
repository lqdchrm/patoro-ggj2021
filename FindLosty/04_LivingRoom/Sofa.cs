using System.Collections.Generic;
using System.Linq;
using LostAndFound.Engine;

namespace LostAndFound.FindLosty._04_LivingRoom
{
    public class Sofa : Thing
    {
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
        public override string Description
        {
            get {
                int number_of_players_sitting = SeatedPlayers.Count();
                int seats_left = NUMBER_OF_SEATS - number_of_players_sitting;

                string description = "The sofa is made of a dark leather.";

                bool is_still_comfortable = (seats_left >= 0);
                description += is_still_comfortable ?
                    " It looks comfortable." :
                    " It would look comfortable if not {number_of_players_sitting} where already sitting on it.";

                description += is_still_comfortable ?
                    $"\n{JoinLast(this.SeatedPlayers)} are already enjoying it." :
                    $"\n{JoinLast(this.SeatedPlayers)} are really thinking {(-1*seats_left)} people should leave the sofa.";
                return description;
            }
        }

        public string ShortDescription()
        {
            string[] description = {
                $"A big {this} on an bright red carpet with {JoinLast(this.SeatedPlayers)} sitting on it.",
            };
            return System.String.Join('\n', description.Where(x => x != null));
        }

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
