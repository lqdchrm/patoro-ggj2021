﻿using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public abstract class BasePlayer<TGame, TRoom, TPlayer, TThing> : BaseContainer<TGame, TRoom, TPlayer, TThing>
        where TGame: BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom: BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer: BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing: BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        internal DiscordChannel _Channel;
        internal DiscordMember _User { get; set; }

        public TRoom Room { get; set; } 

        public BasePlayer(TGame game, string name) : base(game, false, false, name) { }

        public virtual string StatusText => $"[{Name}]";

        public void Mute()
        {
            _User?.ModifyAsync(x => x.Muted = true);
        }

        public void Unmute()
        {
            _User?.ModifyAsync(x => x.Muted = false);
        }

        public void Reply(string msg)
        {
            msg = $"```css\n{msg}```";
            _Channel?.SendMessageAsync(msg);
        }

        public void ReplyWithState(string msg)
        {
            msg = $"```css\n{msg}\nYour Status: {StatusText}```";
            _Channel?.SendMessageAsync(msg);
        }

        public void ReplyImage(string msg)
        {
            msg = $"```\n{msg}\n```";
            _Channel?.SendMessageAsync(msg);
        }

        /// <summary>
        /// TODO: check if needed
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        //public virtual async Task MoveTo(BaseRoom room)
        //{
        //    var oldChanel = this.Room.VoiceChannel;
        //    await room.VoiceChannel.AddOverwriteAsync(this.User, DSharpPlus.Permissions.AccessChannels);
        //    await room.VoiceChannel.PlaceMemberAsync(this.User);
        //    await oldChanel.AddOverwriteAsync(this.User, deny: DSharpPlus.Permissions.AccessChannels);

        //}
    }
}
