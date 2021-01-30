using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;
using LostAndFound.Engine.Events;

namespace LostAndFound.Game
{

    public class LostAndFoundGame : DiscordGame
    {
        private Room CozyHut;
        private Room TheWoods;

        public LostAndFoundGame() : base("LostAndFoundGame") { }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            CozyHut = await AddRoomAsync(new CozyHut());
            TheWoods = await AddRoomAsync(new TheWoods());

            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommandSentEvent e)
        {
            if (e.Player.Room != null)
                await e.Player.Room.HandleCommandAsync(e.Player, e.Command);
        }

        private async void OnPlayerChangedRoom(object sender, PlayerChangedRoomEventArgs e)
        {
            if (e.OldRoom != null)
                await e.OldRoom.SendGameEventAsync($"{e.Player.Name} left {e.OldRoom.Name}");

            if (e.Player.Room != null)
                await e.Player.Room.SendGameEventAsync($"{e.Player.Name} entered {e.Player.Room.Name}");
        }
    }
}
