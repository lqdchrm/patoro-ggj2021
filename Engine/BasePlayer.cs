using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class BasePlayer
    {

        internal protected DiscordChannel Channel { get; set; }
        internal protected DiscordMember User { get; set; }
        internal protected BaseGame Game { get; }


        public string Name { get; }
        public BaseRoom Room { get; internal set; }


        public override string ToString() => Name;

        public BasePlayer(BaseGame game, string name)
        {
            this.Name = name;
            this.Game = game;
        }

        public void SendGameEvent(string msg, bool isImage = false)
        {
            if (isImage)
            {
                msg = $"```\n{msg}\n```";
            }
            else
            {
                msg = $"```css\n{msg}\n```";
            }
            Channel?.SendMessageAsync(msg);
        }

        /// <summary>
        /// TODO: check if needed
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public virtual async Task MoveTo(BaseRoom room)
        {
            var oldChanel = this.Room.VoiceChannel;
            await room.VoiceChannel.AddOverwriteAsync(this.User, DSharpPlus.Permissions.AccessChannels);
            await room.VoiceChannel.PlaceMemberAsync(this.User);
            await oldChanel.AddOverwriteAsync(this.User, deny: DSharpPlus.Permissions.AccessChannels);

        }
    }
}
