using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;
using LostAndFound.Engine.Events;

namespace LostAndFound.Game.FindLosty
{

    public class FindLostyGame : BaseGame
    {
        readonly GameState State;

        readonly FrontYard FrontYard = new FrontYard();
        readonly EntryHall EntryHall = new EntryHall();
        readonly DiningRoom DiningRoom = new DiningRoom();
        readonly Kitchen Kitchen = new Kitchen();
        readonly LivingRoom LivingRoom = new LivingRoom();
        readonly Cellar Cellar = new Cellar();

        public FindLostyGame(string name, DiscordClient client, DSharpPlus.Entities.DiscordGuild guild) : base(name, client, guild)
        {
            State = new GameState(this);
        }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            await AddRoomAsync(FrontYard);
            await AddRoomAsync(EntryHall);
            await AddRoomAsync(DiningRoom);
            await AddRoomAsync(Kitchen);
            await AddRoomAsync(LivingRoom);
            await AddRoomAsync(Cellar);

            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommand e)
        {
            if (e.Player.Room != null)
                await e.Player.Room.HandleCommandAsync(e);
        }

        private async void OnPlayerChangedRoom(object sender, PlayerRoomChange e)
        {
            if (e.OldRoom != null)
                await e.OldRoom.SendGameEventAsync($"{e.Player.Name} left {e.OldRoom.Name}");

            if (e.Player.Room != null)
                await e.Player.Room.SendGameEventAsync($"{e.Player.Name} entered {e.Player.Room.Name}");
        }

        public override BasePlayer CreatePlayer(string userName)
        {
            return new Player(userName, this);
        }
    }
}
