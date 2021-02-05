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
        public override string LookText => (NUMBER_OF_SEATS - SeatedPlayers.Count() - 1) switch
        {

            int i when i > 1 => @$"
                The sofa is made of a dark lather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoing it. {i} seats left.
                ".FormatMultiline(),
            int i when i == 1 => @$"
                The sofa is made of a dark lather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoing it. Only one seat left.
                ".FormatMultiline(),
            int i when i == 0 => @$"
                The sofa is made of a dark lather. It looks comfortable.
                {JoinLast(this.SeatedPlayers)} are already enjoing it. There is no seat left. But you think you still would find a place, when you push some.
                ".FormatMultiline(),
            int i when i == -1 => @$"
                The sofa is made of a dark lather. It would looks comfortable if not to many people where sitting on it.
                {JoinLast(this.SeatedPlayers)} are having an unconfortable time.
                ".FormatMultiline(),
            _ => $@"The sofa is made of a dark lather. It looks comfortable. {NUMBER_OF_SEATS } people can at least take place.",
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
                            {JoinLast(this.SeatedPlayers)} who where just enjoing there time on {this} gleare at you.".FormatMultiline());

                sender.Room.SendText($@"{sender} kicks against the {this}.
                        {JoinLast(this.SeatedPlayers)} who where just enjoing there time on {this} gleare at him.".FormatMultiline(),
                        excludedPlayers: this.SeatedPlayers.Concat(new[] { sender }));

                sender.Room.SendText($@"Your good time is interupted by an unpleasant shake.
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
        public override string OpenText => $"You could open it. But then no one could enjo this very comfortable {this}. And it would get messy.";
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
        public override string TakeText => "Yeah why not. Its not like you may want to keep your back in one pice.";
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

        public override void Use(IPlayer sender, IThing other)
        {
            if (other is null)
            {
                var freeSeets = NUMBER_OF_SEATS - this.SeatedPlayers.Count();
                if (freeSeets <= 0)
                {
                    sender.Reply($"This would get tight.");
                }
                else if (freeSeets == 1)
                {
                    sender.Reply($"You squish yourself on the last free spot on {this}.");

                    sender.Room.SendText($"{sender} squishs himself on the {this}. It's pretty unconfortable now.", sender.Room.Players.Except(this.SeatedPlayers));
                    sender.Room.SendText($"{sender} squishs himself on the {this}. It was already full, now it looks unconfortable.", this.SeatedPlayers.Concat(new[] { sender }));

                    sender.ThingPlayerIsUsingAndHasToStop = this;
                }
                else if (freeSeets == NUMBER_OF_SEATS)
                {
                    sender.Reply($"The {this} is empty what a shame. You will fix this now. You sit down and it is very confortable.");
                    sender.Room.SendText($"{sender} takes place on the {this}. He seems to enjoy it.", sender);
                    sender.ThingPlayerIsUsingAndHasToStop = this;

                }
                else
                {
                    sender.Reply($"You take one of the free spots. It is very confortable.");
                    sender.Room.SendText($"{sender} also sits donw on the {this}. Your in good company.", sender.Room.Players.Except(this.SeatedPlayers));
                    sender.Room.SendText($"{sender} sits down on the {this}.", this.SeatedPlayers.Concat(new[] { sender }));
                    sender.ThingPlayerIsUsingAndHasToStop = this;

                }
            }
            else
            {
                base.Use(sender, other);
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