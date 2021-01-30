using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.Mansion
{
    public class MansionPlayer : BasePlayer<MansionGame, MansionPlayer>
    {
        int Health;


        public MansionPlayer(string name, MansionGame game) : base(game, name) { }

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
            await User.ModifyAsync(x => { x.Muted = Health <= 0; });
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
