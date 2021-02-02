using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Events;
using LostAndFound.FindLosty._00_FrontYard;
//using LostAndFound.FindLosty._01_EntryHall;
//using LostAndFound.FindLosty._02_DiningRoom;
//using LostAndFound.FindLosty._03_Kitchen;
//using LostAndFound.FindLosty._04_LivingRoom;
//using LostAndFound.FindLosty._05_Cellar;

namespace LostAndFound.FindLosty
{
    public class FindLostyGame : BaseGame<FindLostyGame, Room, Player, Thing>
    {
        public readonly GameState State;

        public readonly FrontYard FrontYard;
        //public readonly EntryHall EntryHall;
        //public readonly DiningRoom DiningRoom;
        //public readonly Kitchen Kitchen;
        //public readonly LivingRoom LivingRoom;
        //public readonly Cellar Cellar;

        public FindLostyGame(string name, DiscordClient client, DiscordGuild guild) : base(name, client, guild)
        {
            State = new GameState(this);
            
            // Rooms
            FrontYard = new FrontYard(this);
            //EntryHall = new EntryHall(this);
            //DiningRoom = new DiningRoom(this);
            //Kitchen = new Kitchen(this);
            //LivingRoom = new LivingRoom(this);
            //Cellar = new Cellar(this);
        }

        protected override Player CreatePlayer(string userName) => new Player(userName, this);

        public override async Task StartAsync()
        {
            await base.StartAsync();

            await AddRoomAsync(FrontYard, true);
            //await AddRoomAsync(EntryHall, false);
            //await AddRoomAsync(DiningRoom, false);
            //await AddRoomAsync(Kitchen, false);
            //await AddRoomAsync(LivingRoom, false);
            //await AddRoomAsync(Cellar, false);

            PlayerChangedRoom += OnPlayerChangedRoom;
            CommandSent += OnPlayerCommandSent;
        }

        /// <summary>
        /// search for a thing
        /// 
        /// first search in player's inventory
        /// then search room's inventory
        /// then search player by name
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public BaseThing<FindLostyGame, Room, Player, Thing> GetThing(Player sender, string token)
        {
            if (token is null) return null;

            // find in inventory
            BaseThing<FindLostyGame, Room, Player, Thing> item = null;
            if (!sender.Inventory.TryFind(token, out item, true))
            {
                // find in room
                if (!sender.Room.Inventory.TryFind(token, out item))
                {
                    //search player
                    if (token.Length > 2)
                    {
                        var candidates = Players.Values.Where(p => p.NormalizedName.StartsWith(token)).ToList();
                        if (candidates.Any())
                        {
                            if (candidates.Count() == 1)
                            {
                                item = candidates.First();
                            }
                            else
                            {
                                var names = string.Join(", ", candidates.Select(cand => cand.Name));
                                sender.Reply($"Found multiple players: {names}");
                            }
                        }
                    }

                    // check if current room was meant
                    if (item == null)
                    {
                        if (sender.Room.Name.ToLowerInvariant() == token.ToLowerInvariant())
                            item = sender.Room;
                    }
                }
            }

            return item;
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
            var thing = GetThing(player, first);
            var other = GetThing(player, second);

            Func<string, string> UnknownThing = (name) => new[]
            {
                $"What do you mean by {name}?",
                $"I'm not sure if I understand what {name} is.",
            }.TakeOneRandom();

            switch (cmd)
            {
                // zero or one args
                case "look":
                    if (thing is not null) thing.Look(player);                              // thing found
                    else if (first is not null) player.Reply(UnknownThing(first));          // thing not found => unknown arg
                    else room.Look(player);                                                 // no arg => ask room
                    break;

                case "kick":
                    if (thing is not null) thing.Kick(player);                              // thing found
                    else if (first is not null) player.Reply(UnknownThing(first));          // thing not found => unknown arg
                    else room.Kick(player);                                                 // no arg => ask room
                    break;

                case "listen":
                    if (thing is not null) thing.Listen(player);                            // thing found
                    else if (first is not null) player.Reply(UnknownThing(first));          // thing not found => unknown arg
                    else room.Listen(player);                                               // no arg => ask room
                    break;

                // one arg
                case "open":
                    if (thing is not null) thing.Open(player);                                  // thing found
                    else if (first is not null) player.Reply(UnknownThing(first));              // thing not found => unknown arg
                    else player.Reply("What do you want to open? Please use eg. open door");    // no arg
                    break;

                case "close":
                    if (thing is not null) thing.Close(player);                                 // thing found
                    else if (first is not null) player.Reply(UnknownThing(first));              // thing not found => unknown arg
                    else player.Reply("What do you want to close? Please use eg. open door");   // no arg
                    break;

                // one or two args
                case "take":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Take(player, other);                       // two things
                        else if (second is not null) player.Reply(UnknownThing(second));        // second thing not found
                        else thing.Take(player, room);                                          // one thing => try to take it from room
                    }
                    else if (first is not null) player.Reply(UnknownThing(first));              // first thing not found
                    else player.Reply("What do you want to take? Please use eg. take poo or take hamster from cage");    // no args
                    break;

                case "put":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Put(player, other);                        // two things => put a into b
                        else if (second is not null) player.Reply(UnknownThing(second));        // second thing not found
                        else thing.Put(player, room);                                           // one thing => drop to room
                    }
                    else if (first is not null) player.Reply(UnknownThing(first));              // first thing not found
                    else player.Reply("What do you want to take? Please use eg. take poo or take hamster from cage");    // no args
                    break;

                case "use":
                    if (thing is not null)
                    {
                        if (other is not null) thing.Use(player, other);                        // two things => put a into b
                        else if (second is not null) player.Reply(UnknownThing(second));        // second thing not found
                        else thing.Use(player, room);                                          // one thing => drop to room
                    }
                    else if (first is not null) player.Reply(UnknownThing(first));              // first thing not found
                    else player.Reply("What do you want to take? Please use eg. #take #poo or #take hamster from cage");    // no args
                    break;

                case "help":
                    player.Reply("TODO: some help");
                    break;

                default:
                    player.Reply(new[] {
                        $"Unknown command {cmd}",
                        $"You want to do what?"
                    }.TakeOneRandom());
                    break;

            }
        }

        private void OnPlayerChangedRoom(object sender, PlayerRoomChange<FindLostyGame, Room, Player, Thing> e)
        {
            if (e.OldRoom != null)
            {
                e.Player?.Reply($"You left {e.OldRoom.Name}");
                e.OldRoom.SendText($"{e.Player} left {e.OldRoom.Name}", e.Player);
            }

            if (e.Player.Room != null)
            {
                e.Player?.ReplyWithState($"You entered {e.Player.Room.Name}");
                e.Player.Room.SendText($"[{e.Player}] entered {e.Player.Room.Name}", e.Player);
            }
        }
    }
}
