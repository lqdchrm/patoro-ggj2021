using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.Mansion
{
    public class MansionPlayer : BasePlayer
    {
        internal Characters.BaseCharacter Character { get; set; }


        public MansionPlayer(string name, MansionGame game) : base(game, name) { }


        public async Task SendAsciiArt(string msg)
        {
            msg = $"```css\n{msg}\n```";
            if (Channel != null)
            {
                if (msg.Length > 2000)
                {

                }
                await Channel.SendMessageAsync(msg);
            }
        }
        public async Task SendMessage(string msg)
        {
            //msg = $"```css\n{msg}\n```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }
        public async Task SendHeader(string msg)
        {
            msg = $"**{msg}**";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }
        public async Task SendAdministrativeMessage(string msg)
        {
            msg = $"```css\n{msg}\n```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }


    }
}
