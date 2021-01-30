using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public class Player
    {
        internal DiscordGame Game { get; set; }
        internal DiscordChannel Channel { get; set; }
        internal DiscordMember User { get; set; }

        int Health;

        public string Name { get; }
        public Room Room { get; internal set; }

        public Player(string name) { this.Name = name; }

        public async Task InitAsync()
        {
            this.Health = 3;
            await UpdateStatsAsync();
        }

        public async Task SendGameEventAsync(string msg)
        {
            msg = $"```css\n{msg}\n```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }

        public async Task UpdateStatsAsync()
        {
            await User.ModifyAsync(x => x.Muted = Health <= 0);
            await Channel.ModifyAsync(x => x.Name = $"📜 ${Name} [{Health}]");
        }

        public async Task HitAsync()
        {
            if (this.Health > 0) this.Health--;
            await UpdateStatsAsync();
        }
        
        public async Task HealAsync()
        {
            if (this.Health < 3) this.Health++;
            await UpdateStatsAsync();
        }
    }
}
