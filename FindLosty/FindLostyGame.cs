using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Events;
using LostAndFound.FindLosty._00_FrontYard;
using LostAndFound.FindLosty._01_EntryHall;
using LostAndFound.FindLosty._02_DiningRoom;
//using LostAndFound.FindLosty._03_Kitchen;
//using LostAndFound.FindLosty._04_LivingRoom;
//using LostAndFound.FindLosty._05_Cellar;

namespace LostAndFound.FindLosty
{
    public class FindLostyGame : BaseGame<FindLostyGame, Room, Player, Thing>
    {
        public readonly GameState State;

        public readonly FrontYard FrontYard;
        public readonly EntryHall EntryHall;
        public readonly DiningRoom DiningRoom;
        //public readonly Kitchen Kitchen;
        //public readonly LivingRoom LivingRoom;
        //public readonly Cellar Cellar;

        public FindLostyGame(string name, DiscordClient client, DiscordGuild guild) : base(name, client, guild)
        {
            State = new GameState(this);
            
            // Rooms
            FrontYard = new FrontYard(this);
            EntryHall = new EntryHall(this);
            DiningRoom = new DiningRoom(this);
            //Kitchen = new Kitchen(this);
            //LivingRoom = new LivingRoom(this);
            //Cellar = new Cellar(this);
        }

        protected override Player CreatePlayer(string userName) => new Player(userName, this);

        public override async Task StartAsync()
        {
            await base.StartAsync();

            await AddRoomAsync(FrontYard, true);
            await AddRoomAsync(EntryHall, false);
            await AddRoomAsync(DiningRoom, false);
            //await AddRoomAsync(Kitchen, false);
            //await AddRoomAsync(LivingRoom, false);
            //await AddRoomAsync(Cellar, false);

            PlayerChangedRoom += OnPlayerChangedRoom;
            CommandSent += OnPlayerCommandSent;
        }

        private void OnPlayerCommandSent(object sender, BaseCommand<FindLostyGame, Room, Player, Thing> e)
        {
            if (e.Command == null) return;

            var player = e.Sender;
            var cmd = e.Command;
            var first = e.First;
            var prepo = e.Prepo;
            var second = e.Second;

            var room = player.Room;
            var other = GetThing(player, second);
            var thing = GetThing(player, first, other as BaseContainer<FindLostyGame, Room, Player, Thing>);

            Action<Player, string, BaseThing<FindLostyGame, Room, Player, Thing>>
                ReportUnknown = (player, token, other) =>
            {
                var intro = new[] {
                    $"What do you mean by {token}?",
                    $"There is no {token}",
                    $"If there really was something like a {token}, you could probably do that.",
                }.TakeOneRandom();

                player.Reply(intro);
                GetThing(player, token, other as BaseContainer<FindLostyGame, Room, Player, Thing>, true);
            };

            switch (cmd)
            {
                // zero or one args
                case "look":
                    if (thing is not null) thing.Look(player);                          // thing found
                    else if (first is not null) ReportUnknown(player, first, other);    // thing not found => unknown arg
                    else room.Look(player);                                             // no arg => ask room
                    break;

                case "kick":
                    if (thing is not null) thing.Kick(player);                          // thing found
                    else if (first is not null) ReportUnknown(player, first, other);    // thing not found => unknown arg
                    else room.Kick(player);                                             // no arg => ask room
                    break;

                case "listen":
                    if (thing is not null) thing.Listen(player);                        // thing found
                    else if (first is not null) ReportUnknown(player, first, other);    // thing not found => unknown arg
                    else room.Listen(player);                                           // no arg => ask room
                    break;

                // one arg
                case "open":
                    if (thing is not null) thing.Open(player);                                  // thing found
                    else if (first is not null) ReportUnknown(player, first, other);            // thing not found => unknown arg
                    else player.Reply("What do you want to open? Please use eg. open door");    // no arg
                    break;

                case "close":
                    if (thing is not null) thing.Close(player);                                 // thing found
                    else if (first is not null) ReportUnknown(player, first, other);            // thing not found => unknown arg
                    else player.Reply("What do you want to close? Please use eg. open door");   // no arg
                    break;

                // one or two args
                case "take":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Take(player, other);                   // two things
                        else if (second is not null) ReportUnknown(player, second, other);  // second thing not found
                        else if (player.Inventory.Contains(thing)) player.Reply($"You already have {thing}");   // in own inventory
                        else thing.Take(player, room);                                      // one thing => try to take it from room
                    }
                    else if (first is not null) ReportUnknown(player, first, other);                                    // first thing not found
                    else player.Reply("What do you want to take? Please use eg. take poo or take hamster from cage");   // no args
                    break;

                case "drop":
                case "give":
                case "put":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Put(player, other);                    // two things => put a into b
                        else if (second is not null) ReportUnknown(player, second, other);  // second thing not found
                        else thing.Put(player, room);                                       // one thing => drop to room
                    }
                    else if (first is not null) ReportUnknown(player, first, other);                                        // first thing not found
                    else player.Reply("What do you want to put/give/drop? Please use eg. drop poo or put poo into box");    // no args
                    break;

                case "use":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Use(player, other);                    // two things => put a into b
                        else if (second is not null) ReportUnknown(player, second, other);  // second thing not found
                        else thing.Use(player, null);                                       // one thing
                    }
                    else if (first is not null) ReportUnknown(player, first, other);                                    // first thing not found
                    else player.Reply("What do you want to use? Please use eg. use item or use hamster with cage");     // no args
                    break;

                case "say":
                    if (first is not null)
                    {
                        var msg = string.Join(" ", e.RawArgs);
                        player.Room.Say(msg);
                    }
                    break;

                case "help":
                    player.ReplyWithState($"{HelpText}\nYour are at {player.Room}");
                    break;

                default:
                    var knownCommands = new[] { "look", "kick", "listen", "open", "close", "drop", "take", "give", "put", "use", "help" };
                    var dists = knownCommands.Select(c => (c, d: cmd.Levenshtein(c))).OrderBy(_ => _.d).Take(3);
                    var help = string.Join(", ", dists.Select(_ => $"[{_.c}]"));

                    player.Reply($"Unknown cmd: {cmd}. Did you mean one of these: {help}");
                    break;

            }
        }

        private string HelpText => $@"
            [help]   - This help message
            [look]   - look (something or somebody)*, eg look box
            [listen] - listen (something or somebody)*, eg listen door
            [open]   - open something, eg open door
            [close]  - close something, eg close door
            [take]   - take something (from something or somebody)*, eg take box from player
            [put]    - put something (into something or somebody)*, eg put poo into box
            [kick]   - kick (something or somebody)*, eg kick door
            [use]    - use something (with something)*, eg use poo with box
            [drop]   - drop something, eg drop box
            [give]   - give something to somebody, eg give box to player

            * these are optional
        ".FormatMultiline();

        private void OnPlayerChangedRoom(object sender, PlayerRoomChange<FindLostyGame, Room, Player, Thing> e)
        {
            if (e.OldRoom != null)
            {
                e.Player?.Reply($"You left {e.OldRoom}");
                e.OldRoom.SendText($"{e.Player} left {e.OldRoom}", e.Player);
            }

            if (e.Player.Room != null)
            {
                e.Player?.ReplyWithState($"You entered {e.Player.Room}");
                e.Player.Room.SendText($"{e.Player} entered {e.Player.Room}", e.Player);
            }
        }
    }
}
