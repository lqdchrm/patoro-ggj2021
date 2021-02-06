using LostAndFound.Engine;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public interface IRoom : BaseRoom<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer { }

    public abstract class Room : BaseRoomImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IRoom
    {
        public Room(FindLostyGame game, string roomNumber) : this(game, roomNumber, null) { }
        public Room(FindLostyGame game, string roomNumber, string name = null) : base(game, roomNumber, name) { }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public virtual string Image => null;

        public override void Look(IPlayer sender)
        {
            var friends = this.Players.Where(p => p != sender);
            var friendsNames = string.Join(", ", friends.Select(p => $"{p}"));
            var friendsText = friends.Any()
                ? friends.Count() == 1
                ? $"Your friend {friendsNames} is here."
                : $"Your friends {friendsNames} are here."
                : "You are alone.";

            var content = string.Join(", ", this.Select(i => i.ToString()));

            Task.Run(async () =>
            {
                if (Image is not null)
                {
                    sender.Reply($"{Image}");
                    await Task.Delay(50);
                }
                sender.Reply($"{Description}\n{friendsText}\n{content}");
            });
        }

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */
        public override void Listen(IPlayer sender)
        {
            if (Noises is not null)
            {
                sender.Reply($"{Noises}");
            } else
            {
                base.Listen(sender);
            }
        }
    }
}
