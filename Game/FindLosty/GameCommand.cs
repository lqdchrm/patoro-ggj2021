using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class GameCommand
    {
        private readonly PlayerCommand inner;
        public GameCommand(PlayerCommand inner) => this.inner = inner;
        public string Message => inner?.Message;
        public Player Player => inner?.Player as Player;
        public IList<Player> Mentions => inner?.Mentions.Cast<Player>().ToList();
        public string Command => inner?.Command;
        public IList<string> Args => inner?.Args;
    }
}
