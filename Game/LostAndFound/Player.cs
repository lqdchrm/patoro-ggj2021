using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.LostAndFound
{
    public class Player : BasePlayer
    {
        private int health;
        int Health
        {
            get => health;
            set
            {
                if (User != null)
                {
                    if (value == 0 && health > 0)
                        User.ModifyAsync(x => x.Muted = true);
                }
                health = value;
            }
        }

        public Player(string name, LostAndFoundGame game) : base(game, name) { }

        public override string ToString()
        {
            return $"{Name} { string.Join("", Enumerable.Repeat(Emojis.Heart, Health)) }";
        }

        public override Task InitAsync()
        {
            this.Health = 3;
            return Task.CompletedTask;
        }

        public async Task SendGameEventAsync(string msg)
        {
            msg = $"```css\n{msg}\n```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }

        public async Task HitAsync()
        {
            if (this.Health > 0)
                this.Health--;
            await SendGameEventAsync("Your were hit");
        }

        public Task HealAsync()
        {
            if (this.Health < 3) this.Health++;
            return Task.CompletedTask;
        }
    }
}
