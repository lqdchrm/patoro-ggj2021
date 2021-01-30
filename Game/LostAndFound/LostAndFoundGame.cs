using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;
using LostAndFound.Engine.Events;

namespace LostAndFound.Game.LostAndFound
{

    public class LostAndFoundGame : BaseGame<LostAndFoundGame, Player, CommonRoom>
    {
        private CommonRoom CozyHut;
        private CommonRoom TheWoods;

        public LostAndFoundGame(string name, DiscordClient client, DSharpPlus.Entities.DiscordGuild guild) : base(name, client, guild) { }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            CozyHut = await AddRoomAsync(new CozyHut());
            TheWoods = await AddRoomAsync(new TheWoods());

            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommand<LostAndFoundGame, Player, CommonRoom> e)
        {
            if (e.Player.Room != null)
                await e.Player.Room.HandleCommandAsync(e.Player, e.Command);
        }

        private async void OnPlayerChangedRoom(object sender, PlayerRoomChange<LostAndFoundGame, Player, CommonRoom> e)
        {
            if (e.OldRoom != null)
                await e.OldRoom.SendGameEventAsync($"{e.Player.Name} left {e.OldRoom.Name}");

            if (e.Player.Room != null)
                await e.Player.Room.SendGameEventAsync($"{e.Player.Name} entered {e.Player.Room.Name}");
        }

        public override Player CreatePlayer(string userName)
        {
            return new Player(userName, this);
        }
    }
}
