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
        public readonly GameState State;

        public readonly FrontYard FrontYard = new FrontYard();
        public readonly EntryHall EntryHall = new EntryHall();
        public readonly DiningRoom DiningRoom = new DiningRoom();
        public readonly Kitchen Kitchen = new Kitchen();
        public readonly LivingRoom LivingRoom = new LivingRoom();
        public readonly Cellar Cellar = new Cellar();

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

        protected override async Task ShowOpening(Func<string, Task> post)
        {
            await post("#Find Losty#");
            await post(@"
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
            await post(@"
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

            await post(@"
This is a cooperative textbase adventure.

When you enter the first Voice chat of this game,
a text channel only accessable for you will be created.

This channel will tell you everything that happesn in the game
and your textinput will controle your character.

While progressing the game you will unlock new rooms.
You can navigate between the rooms by switching the voice channels.

Happy gaming :)
".FormatMultiline());


        }



        private void OnPlayerCommandSent(object sender, PlayerCommand e)
        {
            if (e.Player.Room is CommonRoom room)
            {
                room.HandleCommand(e);
            }
            else if (e.Player is Player player)
            {
                player.SendGameEvent("Command ignored, Please join a Game Voice-Channel.");
            }

        }

        private void OnPlayerChangedRoom(object sender, PlayerRoomChange e)
        {
            if (e.OldRoom != null)
            {
                (e.Player as Player)?.SendGameEvent($"You left {e.OldRoom.Name}");
                e.OldRoom.SendGameEvent($"[{e.Player}] left {e.OldRoom.Name}", e.Player);
            }

            if (e.Player.Room != null)
            {
                (e.Player as Player)?.SendGameEventWithState($"You entered {e.Player.Room.Name}");
                e.Player.Room.SendGameEvent($"[{e.Player}] entered {e.Player.Room.Name}", e.Player);
            }
        }

        public override BasePlayer CreatePlayer(string userName)
        {
            return new Player(userName, this);
        }
    }
}
