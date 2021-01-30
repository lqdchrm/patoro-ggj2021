using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;
using LostAndFound.Engine.Events;

namespace LostAndFound.Game.Mansion
{

    public class MansionGame : DiscordGame<MansionGame, MansionPlayer>
    {
        private MansionRoom CozyHut;
        private MansionRoom TheWoods;

        private Characters.John John = new Characters.John();
        private Characters.Kathrin Kathrin = new Characters.Kathrin();
        private Characters.Maria Maria = new Characters.Maria();
        private Characters.Paul Paul = new Characters.Paul();

        private readonly Characters.BaseCharacter[] Characters;

        private bool gameStarted;

        public MansionGame(string name, DiscordClient client, DSharpPlus.Entities.DiscordGuild guild) : base(name, client, guild)
        {

            Characters = new Characters.BaseCharacter[] { John, Kathrin, Maria, Paul };
        }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            CozyHut = await AddRoomAsync(new Rooms.Void());
            TheWoods = await AddRoomAsync(new Rooms.Laboratory());

            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommandSentEvent<MansionGame, MansionPlayer> e)
        {
            if (e.Player.Room != null)
                await e.Player.Room.HandleCommandAsync(e.Player, e.Command);
        }

        public override bool IsEverythingCommand => false;

        protected override async Task NewPlayer(MansionPlayer player)
        {
            if (gameStarted)
            {
                await player.SendGameEventAsync("Sorry but the game already started.");

                return;
            }
            if (gameStarted)
            {
                await player.SendGameEventAsync("Sorry but the game already started.");
            }

            await player.SendGameEventAsync("Welcome.\n You can write in this channel everything you do.\nAnd you will find out here what happens to you.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await player.SendGameEventAsync("You can alos use the voice chat to chat with character in the same location.");
            await Task.Delay(TimeSpan.FromSeconds(1));
            await player.SendGameEventAsync("Please do not leave the voice chat manually and stay in this text chat to have an imersive story ;)");

        }

        private async void OnPlayerChangedRoom(object sender, PlayerChangedRoomEventArgs<MansionGame, MansionPlayer> e)
        {
            if (e.OldRoom != null)
                await e.OldRoom.SendGameEventAsync($"{e.Player.Name} left {e.OldRoom.Name}");

            if (e.Player.Room != null)
                await e.Player.Room.SendGameEventAsync($"{e.Player.Name} entered {e.Player.Room.Name}");
        }

        public override MansionPlayer CreatePlayer(string userName)
        {
            return new MansionPlayer(userName, this);
        }
    }
}
