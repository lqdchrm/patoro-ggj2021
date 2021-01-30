using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BasePlayer<TGame, TPlayer, TRoom>
        where TGame : BaseGame<TGame, TPlayer, TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer, TRoom>
                where TRoom : BaseRoom<TGame, TPlayer, TRoom>

    {

        internal protected DiscordChannel Channel { get; set; }
        internal protected DiscordMember User { get; set; }

        public string Name { get; }


        internal protected TGame Game { get; }

        public TRoom Room { get; internal set; }
        public BasePlayer(TGame game, string name)
        {
            this.Name = name;
            this.Game = game;
        }
        public virtual Task InitAsync() => Task.CompletedTask;

        public virtual async Task MoveTo(TRoom room)
        {
            var oldChanel = this.Room.VoiceChannel;
            await room.VoiceChannel.AddOverwriteAsync(this.User, DSharpPlus.Permissions.AccessChannels);
            await room.VoiceChannel.PlaceMemberAsync(this.User);
            await oldChanel.AddOverwriteAsync(this.User, deny: DSharpPlus.Permissions.AccessChannels);

        }
    }
}
