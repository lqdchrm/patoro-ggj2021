using System;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Game.FindLosty
{
    public class CommandParser
    {

        private int index;
        private readonly object[] objects;
        private readonly IList<string> commandList;

        public CommandParser(object[] vs, IList<string> commandList)
        {
            this.objects = vs;
            this.commandList = commandList;
        }

        public class CommandParserBuilder
        {
            private List<object> argumentList = new List<object>();
            private IList<string> commandList;

            public CommandParserBuilder(IList<string> commandList)
            {
                this.commandList = commandList;
            }

            public CommandParser Build()
            {
                return new CommandParser(argumentList.ToArray(), this.commandList);
            }

            public CommandParserBuilder For(CommonRoom room, bool includeItems = true)
            {
                argumentList.AddRange(room.KnownThings);
                if (includeItems)
                    argumentList.AddRange(room.Inventory.Values);
                return this;
            }
            public CommandParserBuilder For(Inventory inventory)
            {
                argumentList.AddRange(inventory.Values);
                return this;
            }

            /// <summary>
            /// Will add parsing for this Players Name
            /// </summary>
            /// <param name="player"></param>
            /// <returns></returns>
            public CommandParserBuilder For(Player player)
            {
                argumentList.Add(player);
                return this;
            }

            public CommandParserBuilder For(IEnumerable<Player> players)
            {
                foreach (var p in players)
                {
                    this.For(p);
                }
                return this;
            }
            public CommandParserBuilder For(params Player[] players)
            {
                return For(players as IEnumerable<Player>);
            }



        }

        public Player TakePlayer()
        {
            return objects.OfType<Player>().Where(x => TryTake(x.Name)).FirstOrDefault();
        }
        public Item TakeItem()
        {
            return objects.OfType<Item>().Where(x => TryTake(x.Name)).FirstOrDefault();
        }
        public string TakeString(params string[] strs)
        {
            return strs.Where(x => TryTake(x.AsSpan())).FirstOrDefault();
        }

        private bool TryTake(ReadOnlySpan<char> name)
        {
            var splittedName = name.Split(' ');
            var currentIndex = index;
            foreach (var part in splittedName)
            {
                if (index >= commandList.Count)
                    return false;

                if (commandList[index] != name[part])
                    return false;
                currentIndex++;
            }
            this.index = currentIndex;
            return true;
        }
    }
}
