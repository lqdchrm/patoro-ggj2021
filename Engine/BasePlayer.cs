using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BasePlayer<TGame, TPlayer, TRoom>
        where TGame : DiscordGame<TGame, TPlayer, TRoom>
        where TPlayer : BasePlayer<TGame, TPlayer, TRoom>
                where TRoom : Room<TGame, TPlayer, TRoom>

    {

        internal DiscordChannel Channel { get; set; }
        internal DiscordMember User { get; set; }

        public string Name { get; }


        internal TGame Game { get; }

        public TRoom Room { get; internal set; }
        public BasePlayer(TGame game, string name)
        {
            this.Name = name;
            this.Game = game;
        }
        public virtual Task InitAsync() => Task.CompletedTask;

        internal async Task MoveTo(TRoom room)
        {
            var oldChanel = this.Room.VoiceChannel;
            await room.VoiceChannel.AddOverwriteAsync(this.User, DSharpPlus.Permissions.AccessChannels);
            await room.VoiceChannel.PlaceMemberAsync(this.User);
            await oldChanel.AddOverwriteAsync(this.User, deny: DSharpPlus.Permissions.AccessChannels);

        }
    }
}
