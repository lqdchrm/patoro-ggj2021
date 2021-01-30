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

        readonly public FrontYard FrontYard = new FrontYard();
        readonly public EntryHall EntryHall = new EntryHall();
        readonly public DiningRoom DiningRoom = new DiningRoom();
        readonly public Kitchen Kitchen = new Kitchen();
        readonly public LivingRoom LivingRoom = new LivingRoom();
        readonly public Cellar Cellar = new Cellar();

        public FindLostyGame(string name, DiscordClient client, DSharpPlus.Entities.DiscordGuild guild) : base(name, client, guild)
        {
            State = new GameState(this);
        }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            await AddRoomAsync(FrontYard, true);
            await AddRoomAsync(EntryHall, false);
            await AddRoomAsync(DiningRoom, false);
            await AddRoomAsync(Kitchen, false);
            await AddRoomAsync(LivingRoom, false);
            await AddRoomAsync(Cellar, false);

            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommand e)
        {
            if (e.Player.Room != null)
            {
                await e.Player.Room.HandleCommandAsync(e);
            } else if (e.Player is Player player)
            {
                player.SendGameEventAsync("Command ignored, Please join a Game Voice-Channel.");
            }
        
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
