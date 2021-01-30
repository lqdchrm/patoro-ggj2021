using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;

using LostAndFound.Engine;
using LostAndFound.Engine.Events;
using LostAndFound.Game.Mansion.Rooms;

namespace LostAndFound.Game.Mansion
{

    public class MansionGame : BaseGame<MansionGame, MansionPlayer, MansionRoom>
    {
        internal Rooms.Void Void { get; private set; }
        internal Laboratory Laboratory { get; private set; }

        internal DinginRoom DiningRoom { get; private set; }
        internal Kitchen Kitchen { get; private set; }
        internal LivvingRoom LivingRoom { get; private set; }
        internal Hall Hall { get; private set; }

        public override bool IsEverythingCommand => true;


        private Characters.John John = new Characters.John();
        private Characters.Kathrin Kathrin = new Characters.Kathrin();
        private Characters.Maria Maria = new Characters.Maria();
        private Characters.Paul Paul = new Characters.Paul();

        private System.Threading.SemaphoreSlim characterSelectionSemaphore = new System.Threading.SemaphoreSlim(1);

        private readonly Characters.BaseCharacter[] Characters;

        private bool gameStarted;


        public MansionGame(string name, DiscordClient client, DSharpPlus.Entities.DiscordGuild guild) : base(name, client, guild)
        {

            Characters = new Characters.BaseCharacter[] { John, Kathrin, Maria, Paul };
        }

        public override async Task StartAsync()
        {
            await base.StartAsync();

            Void = await AddRoomAsync(new Rooms.Void());
            Laboratory = await AddRoomAsync(new Rooms.Laboratory(), deney: Permissions.AccessChannels);
            DiningRoom = await AddRoomAsync(new Rooms.DinginRoom(), deney: Permissions.AccessChannels);
            Kitchen = await AddRoomAsync(new Rooms.Kitchen(), deney: Permissions.AccessChannels);
            LivingRoom = await AddRoomAsync(new Rooms.LivvingRoom(), deney: Permissions.AccessChannels);
            Hall = await AddRoomAsync(new Rooms.Hall(), deney: Permissions.AccessChannels);


            PlayerChangedRoom += OnPlayerChangedRoom;
            PlayerCommandSent += OnPlayerCommandSent;
        }

        private async void OnPlayerCommandSent(object sender, PlayerCommand<MansionGame, MansionPlayer, MansionRoom> e)
        {
            if (e.Player.Room != null)
                await e.Player.Room.HandleCommandAsync(e.Player, e.Command);
        }


        protected override async Task NewPlayer(MansionPlayer player)
        {
            if (gameStarted)
            {
                await player.SendMessage("Sorry but the game already started.");

                return;
            }

            await characterSelectionSemaphore.WaitAsync();
            try
            {
                var freeCharacter = this.Characters.FirstOrDefault(x => x.Player is null);

                if (freeCharacter is null)
                {
                    await player.SendMessage("Sorry but there is no free character left.");
                    return;
                }
                player.Character = freeCharacter;
                freeCharacter.Player = player;
            }
            finally
            {
                characterSelectionSemaphore.Release();
            }
            await player.SendAdministrativeMessage("Welcome.\n You can write in this channel everything you do.\nAnd you will find out here what happens to you.");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await player.SendAdministrativeMessage("You can alos use the voice chat to chat with character in the same location.");
            await Task.Delay(TimeSpan.FromSeconds(1));
            await player.SendAdministrativeMessage("Please do not leave the voice chat manually and stay in this text chat to have an imersive story ;)");
            await Task.Delay(TimeSpan.FromSeconds(5));
            await player.SendHeader("Lost in the mansion");

            await Task.Delay(TimeSpan.FromSeconds(2));
            await player.SendAsciiArt(@"
               *         .              *            _.---._      
                             ___   .            ___.'       '.   *
        .              _____[LLL]______________[LLL]_____     \
                      /     [LLL]              [LLL]     \     |
                     /____________________________________\    |    .
                      )==================================(    /
     .      *         '|I .-. I .-. I .--. I .-. I .-. I|'  .'
                  *    |I |+| I |+| I |. | I |+| I |+| I|-'`       *
                       |I_|+|_I_|+|_I_|__|_I_|+|_I_|+|_I|      .
              .       /_I_____I_____I______I_____I_____I_\
                       )================================(   *
       *         _     |I .-. I .-. I .--. I .-. I .-. I|          *
                |u|  __|I |+| I |+| I |<>| I |+| I |+| I|    _         .
           __   |u|_|uu|I |+| I |+| I |~ | I |+| I |+| I| _ |U|     _
       .  |uu|__|u|u|u,|I_|+|_I_|+|_I_|__|_I_|+|_I_|+|_I||n|| |____|u|
          |uu|uu|_,.-' /I_____I_____I______I_____I_____I\`'-. |uu u|u|__
          |uu.-'`      #############(______)#############    `'-. u|u|uu|
         _.'`              ~""^""~   (________)   ~""^""^~           `'-.|uu|
");
            await player.SendAsciiArt(@"
      ,''          .'    _                             _ `'-.        `'-.
  ~""^""~    _,'~""^""~    _( )_                         _( )_   `'-.        ~""^""~
      _  .'            |___|                         |___|      ~""^""~     _
    _( )_              |_|_|          () ()          |_|_|              _( )_
    |___|/\/\/\/\/\/\/\|___|/\/\/\/\/\|| ||/\/\/\/\/\|___|/\/\/\/\/\/\/\|___|
    |_|_|\/\/\/\/\/\/\/|_|_|\/\/\/\/\/|| ||\/\/\/\/\/|_|_|\/\/\/\/\/\/\/|_|_|
    |___|/\/\/\/\/\/\/\|___|/\/\/\/\/\|| ||/\/\/\/\/\|___|/\/\/\/\/\/\/\|___|
    |_|_|\/\/\/\/\/\/\/|_|_|\/\/\/\/\/[===]\/\/\/\/\/|_|_|\/\/\/\/\/\/\/|_|_|
    |___|/\/\/\/\/\/\/\|___|/\/\/\/\/\|| ||/\/\/\/\/\|___|/\/\/\/\/\/\/\|___|
    |_|_|\/\/\/\/\/\/\/|_|_|\/\/\/\/\/|| ||\/\/\/\/\/|_|_|\/\/\/\/\/\/\/|_|_|
    |___|/\/\/\/\/\/\/\|___|/\/\/\/\/\|| ||/\/\/\/\/\|___|/\/\/\/\/\/\/\|___|
~""""~|_|_|\/\/\/\/\/\/\/|_|_|\/\/\/\/\/|| ||\/\/\/\/\/|_|_|\/\/\/\/\/\/\/|_lc|~""""~
   [_____]            [_____]                       [_____]            [_____]
");
            await player.MoveTo(this.Rooms[player.Character.StartLocation]);

            foreach (var p in player.Character.Prolog)
            {
                await player.SendMessage(p);
                await Task.Delay(TimeSpan.FromSeconds(3));
            }

        }

        private async void OnPlayerChangedRoom(object sender, PlayerRoomChange<MansionGame, MansionPlayer, MansionRoom> e)
        {
            //if (e.OldRoom != null)
            //    await e.OldRoom.SendGameEventAsync($"{e.Player.Name} left {e.OldRoom.Name}");

            //if (e.Player.Room != null)
            //    await e.Player.Room.SendGameEventAsync($"{e.Player.Name} entered {e.Player.Room.Name}");
        }

        public override MansionPlayer CreatePlayer(string userName)
        {
            return new MansionPlayer(userName, this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.characterSelectionSemaphore.Dispose();
        }
    }
}
