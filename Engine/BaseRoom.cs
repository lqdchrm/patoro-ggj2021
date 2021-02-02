using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public abstract class BaseRoom<TGame, TRoom, TPlayer, TThing> : BaseContainer<TGame, TRoom, TPlayer, TThing>
        where TGame: BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom: BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer: BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        internal DiscordChannel _VoiceChannel { get; set; }

        public bool IsVisible { get; set; }
        public Task Show() => this.Game._SetRoomVisibility(this, true);
        public Task Hide() => this.Game._SetRoomVisibility(this, false);

        public IEnumerable<TPlayer> Players => Game.Players.Values.Where(p => p.Room == this).ToList();

        #region Visibility

        #endregion

        public BaseRoom(TGame game, string name = null) : base(game, false, true, name) { }

        public void SendText(string msg, params TPlayer[] excludedPlayers)
        {
            msg = $"```css\n{msg}\n```";
            foreach(var player in Players.Where(p => !excludedPlayers.Contains(p)))
            {
                player._Channel?.SendMessageAsync(msg);
            }
        }
    }
}
