using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class GameState
    {
        private readonly FindLostyGame Game;

        public GameState(FindLostyGame game) { this.Game = game; }

        public bool CanEnterEntryHall => Game.EntryHall.IsFrontDoorOpen;

        public static bool FrontDoorOpen;
    }
}
