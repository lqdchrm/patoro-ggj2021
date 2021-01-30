using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BasePlayer
    {

        internal protected DiscordChannel Channel { get; set; }
        internal protected DiscordMember User { get; set; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        internal protected BaseGame Game { get; }

        public BaseRoom Room { get; internal set; }
        public BasePlayer(BaseGame game, string name)
        {
            this.Name = name;
            this.Game = game;
        }
        public virtual Task InitAsync() => Task.CompletedTask;

        public virtual async Task MoveTo(BaseRoom room)
        {
            var oldChanel = this.Room.VoiceChannel;
            await room.VoiceChannel.AddOverwriteAsync(this.User, DSharpPlus.Permissions.AccessChannels);
            await room.VoiceChannel.PlaceMemberAsync(this.User);
            await oldChanel.AddOverwriteAsync(this.User, deny: DSharpPlus.Permissions.AccessChannels);

        }
    }
}
