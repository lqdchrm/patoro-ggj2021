using DSharpPlus;
using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Cnsole;
using LostAndFound.Engine.Discord;
using LostAndFound.Engine.Events;
using LostAndFound.FindLosty._00_FrontYard;
using LostAndFound.FindLosty._01_EntryHall;
using LostAndFound.FindLosty._02_DiningRoom;
using LostAndFound.FindLosty._03_Kitchen;
using LostAndFound.FindLosty._04_LivingRoom;
using LostAndFound.FindLosty._05_Cellar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public interface IFindLostyGame : BaseGame<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>
    {
        FrontYard FrontYard { get; }
        EntryHall EntryHall { get; }
        DiningRoom DiningRoom { get; }
        LivingRoom LivingRoom { get; }
        Kitchen Kitchen { get; }
        Cellar Cellar { get; }
    }

    public class FindLostyGame : BaseGameImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IFindLostyGame
    {
        #region Game Factory

        public static FindLostyGame Discord(string name, DiscordClient client, DiscordGuild guild)
        {
            return new FindLostyGame(name,
                new DiscordEngine<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>(client, guild));
        }

        public static FindLostyGame Terminal(string mode, string filePath = null)
        {
            IEnumerable<string> scriptLines = null;
            if (filePath != null && File.Exists(filePath))
                scriptLines = File.ReadAllLines(filePath);

            return new FindLostyGame("FindLosty",
                new TerminalEngine<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>(mode, scriptLines));
        }
        #endregion

        public FrontYard FrontYard { get; init; }
        public EntryHall EntryHall { get; init; }
        public DiningRoom DiningRoom { get; init; }
        public LivingRoom LivingRoom { get; init; }
        public Kitchen Kitchen { get; init; }
        public Cellar Cellar { get; init; }

        private FindLostyGame(string name, BaseEngine<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> engine) : base(name, engine)
        {
            // Rooms
            this.FrontYard = new FrontYard(this);
            this.EntryHall = new EntryHall(this);
            this.DiningRoom = new DiningRoom(this);
            this.Kitchen = new Kitchen(this);
            this.LivingRoom = new LivingRoom(this);
            this.Cellar = new Cellar(this);
        }

        public override Player CreateAndAddPlayer(string name)
        {
            var player = new Player(this, name);
            this._Players.Add(player.Name, player);
            return player;
        }

        public override async Task InitAsync()
        {
            PlayerChangedRoom += OnPlayerChangedRoom;
            CommandSent += OnPlayerCommandSent;

            await AddRoomAsync(this.FrontYard, true);
            await AddRoomAsync(this.EntryHall, false);
            await AddRoomAsync(this.DiningRoom, false);
            await AddRoomAsync(this.LivingRoom, false);
            await AddRoomAsync(this.Kitchen, false);
            await AddRoomAsync(this.Cellar, false);
        }

        private void ReportUnknown(IPlayer sender, string token, IThing other)
        {
            var intro = new[] {
                $"What do you mean by {token}?",
                $"There is no {token}",
                $"If there really was something like a {token}, you could probably do that.",
            }.TakeOneRandom();

            sender.Reply(intro);
            GetThing(sender, token, other as IContainer, true);
        }

        private void OnPlayerCommandSent(object sender, BaseCommand<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> e)
        {
            if (e.Command == null) return;

            var player = e.Sender;
            var cmd = e.Command;
            var first = e.First;
            var prepo = e.Prepo;
            var second = e.Second;

            var room = player.Room;
            var other = GetThing(player, second);
            var thing = GetThing(player, first, other as IContainer) ?? GetThing(player, prepo);

            // check if player is using something at the moment
            var commandsUnusableDuringUse = new[] { "kick", "open", "close", "drop", "give", "put", "use", "yes" };

            if (player.ThingPlayerIsUsingAndHasToStop != null && commandsUnusableDuringUse.Contains(cmd))
            {
                player.Reply($"You are still using {player.ThingPlayerIsUsingAndHasToStop}. Please stop before doing anything else.");
                return;
            }

            if (cmd != "yes" && cmd != "no")
                player.TheThingThatAskedAQuestion = null;

            // Main command selection
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
                        if (other is not null) thing.Take(player, other);                       // two things
                        else if (second is not null) ReportUnknown(player, second, other);      // second thing not found
                        else if (player.Contains(thing)) player.Reply($"You already have {thing}");   // in own inventory
                        else thing.Take(player, room);                                          // one thing => try to take it from room
                    }
                    else if (first is not null) ReportUnknown(player, first, other);            // first thing not found
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
                        else if (thing is PinPad pinpad && second is string) pinpad.Use(player, second);
                        else if (thing is Phone phone && second is string) phone.Use(player, second);
                        else if (second is not null) ReportUnknown(player, second, other);  // second thing not found
                        else thing.Use(player);                                             // one thing
                    }
                    else if (first is not null) ReportUnknown(player, first, other);                                    // first thing not found
                    else player.Reply("What do you want to use? Please use eg. use item or use hamster with cage");     // no args
                    break;

                case "stop":
                    var inUse = player.ThingPlayerIsUsingAndHasToStop;
                    if (inUse != null)
                    {
                        player.Reply($"You have stopped using {inUse}");
                        player.ThingPlayerIsUsingAndHasToStop = null;
                    }
                    break;

                case "say":
                    if (first is not null)
                    {
                        var msg = string.Join(" ", e.RawArgs);
                        player.Room.Say(msg);
                    }
                    break;

                case "cheat":
                    if (first == "open")
                    {
                        var roomToOpen = this.Rooms.Values.FirstOrDefault(room => room.RoomNumber == prepo);
                        if (roomToOpen is not null)
                        {
                            roomToOpen.Show(true);
                        }
                        else if (prepo == "all")
                        {
                            foreach (var _room in this.Rooms.Values)
                                _room.Show(true);
                        }
                    }
                    break;

                case "goto":
                    {
                        var possibleRoomToGo = this.Rooms.Values.Where(x => x.IsVisible)
                            .Where(room => room.RoomNumber == first
                                    || (first.Length >= 2 && room.Name.StartsWith(first, StringComparison.OrdinalIgnoreCase)));
                        if (possibleRoomToGo.Skip(1).Any())
                            player.Reply($"Multiple possible rooms found for {first}: {string.Join(", ", possibleRoomToGo)}.");
                        else if (possibleRoomToGo.Any())
                            player.MoveTo(possibleRoomToGo.First());
                        else
                            player.Reply($"Unknown Room: {first}.");
                    }
                    break;

                case "run":
                    {
                        if (this.EntryHall.MetalDoor.IsDynamiteUsed && !this.EntryHall.MetalDoor.IsOpen)
                            player.MoveTo(new IRoom[] { this.FrontYard, this.DiningRoom, this.LivingRoom }.TakeOneRandom());
                    }
                    break;
                case "no":
                case "yes":
                    {
                        if (first is not null)
                        {
                            player.Reply("What? Just 'yes' or 'no' usually is enough.");
                        }
                        else
                        {
                            var the_thing_that_asked = player.TheThingThatAskedAQuestion;
                            if (the_thing_that_asked == null)
                            {
                                player.Reply($"What exactly are you saying {cmd} to?");
                            }
                            else
                            {
                                the_thing_that_asked.answer(player, cmd);
                            }
                        }
                    }
                    break;

                case "help":
                    player.ReplyWithState($"{HelpText}\nYour are at {player.Room}");
                    break;

                default:
                    var knownCommands = new[] { "look", "listen", "open", "close", "take", "put", "kick", "use", "stop", "drop", "give", "goto", "help" };
                    var dists = knownCommands.Select(c => (c, d: cmd.Levenshtein(c))).OrderBy(_ => _.d).Take(3);
                    var help = string.Join(", ", dists.Select(_ => $"[{_.c}]"));

                    player.Reply($"Unknown cmd: {cmd}. Did you mean one of these: {help}");
                    break;

            }
        }

        private static string HelpText => $@"
            [help]   - This help message
            [look]   - look (something or somebody)*, eg look box
            [listen] - listen (something or somebody)*, eg listen door
            [open]   - open something, eg open door
            [close]  - close something, eg close door
            [take]   - take something (from something or somebody)*, eg take box from player
            [put]    - put something (into something or somebody)*, eg put poo into box
            [kick]   - kick (something or somebody)*, eg kick door
            [use]    - use something (with something)*, eg use poo with box
            [stop]   - stops your current action
            [drop]   - drop something, eg drop box
            [give]   - give something to somebody, eg give box to player
            [goto]   - go to another room, eg goto kitchen

            * these are optional
        ".FormatMultiline();

        private void OnPlayerChangedRoom(object sender, PlayerRoomChange<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> e)
        {
            var oldRoom = e.OldRoom;
            var newRoom = e.Player.Room;

            if (oldRoom is not null)
            {
                // stop doing things on room change
                if (e.Player?.ThingPlayerIsUsingAndHasToStop != null)
                    e.Player.ThingPlayerIsUsingAndHasToStop = null;

                // drop things if leaving game
                if (newRoom is null)
                {
                    var stuff = e.Player.ToList();
                    foreach (var item in stuff)
                        e.Player.Transfer(item, oldRoom);

                    e.Player.Room = oldRoom;
                    e.Player?.Reply($"You left your stuff in {oldRoom}.");
                    e.Player.Room = newRoom;

                    oldRoom.Game.BroadcastMsg($"{e.Player} left {oldRoom} and dropped {string.Join(", ", stuff)}.", e.Player);
                } else
                {
                    oldRoom.BroadcastMsg($"{e.Player} left {oldRoom}", e.Player);
                }
            }

            if (newRoom != null)
            {
                newRoom.BroadcastMsg($"{e.Player} entered {e.Player.Room}", e.Player);
                RaiseCommand(new BaseCommand<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>(e.Player, "look"));
            }
        }
    }
}
