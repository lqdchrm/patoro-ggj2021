﻿using Patoro.TAE;
using System.Linq;
using System.Threading.Tasks;

namespace FindLosty
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
        public override void Look(IPlayer sender)
        {
            var description = Description.ToString();

            var friends = this.Players.Where(p => p != sender);
            var friendsNames = string.Join(", ", friends.Select(p => $"{p}"));
            var friendsText = friends.Any()
                ? friends.Count() == 1
                ? $"\n\tYour friend {friendsNames} is here."
                : $"\n\tYour friends {friendsNames} are here."
                : "\n\tCurrently you are alone at this place.";

            var content = this.Select(i => i.ToString());
            var contentText = content.Any() ? $"\n\tThings: {string.Join(", ", content)}" : "";

            var rooms = Game.Rooms.Values.Where(r => r.IsVisible).Except(sender.Room.Yield());
            var roomsText = rooms.Any() ? $"\n\tRooms: {string.Join(", ", rooms)}" : "";

            Task.Run(async () =>
            {
                if (Image is not null)
                {
                    sender.ReplyImage($"{Image}");
                    await Task.Delay(50);
                }
                sender.Reply($"{description}\n{friendsText}{contentText}{roomsText}");
            }).Wait();
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
