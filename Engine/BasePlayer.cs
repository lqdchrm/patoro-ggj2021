using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BasePlayer<TGame, TPlayer>
        where TGame : DiscordGame<TGame, TPlayer>
        where TPlayer : BasePlayer<TGame, TPlayer>
    {

        internal DiscordChannel Channel { get; set; }
        internal DiscordMember User { get; set; }

        public string Name { get; }


        internal TGame Game { get; }

        public Room<TGame, TPlayer> Room { get; internal set; }
        public BasePlayer(TGame game, string name)
        {
            this.Name = name;
            this.Game = game;
        }
        public virtual Task InitAsync() => Task.CompletedTask;

    }
}
