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

        private bool frontDoorOpen = false;
        public bool FrontDoorOpen
        {
            get => frontDoorOpen;
            set
            {
                if (!frontDoorOpen)
                {
                    frontDoorOpen = true;
                    Game.EntryHall.Show();
                }
            }
        }
    }
}
