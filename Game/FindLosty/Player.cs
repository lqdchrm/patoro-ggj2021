using DSharpPlus.Entities;
using LostAndFound.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
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

        public readonly Inventory Inventory = new Inventory();
        public new CommonRoom Room => (base.Room as CommonRoom);

        public Player(string name, FindLostyGame game) : base(game, name) { }

        public override string ToString()
        {
            var health = string.Join("", Enumerable.Repeat(Emojis.Heart, Health));
            var items = string.Join("", Inventory.Values);
            return $"{Name} {health} {items}";
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

        public async Task SendGameEventWithStateAsync(string msg)
        {
            msg = $"```css\n{msg}\nYour Status: {this}```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }

        public async Task ShowStatusAsync(string msg)
        {
            msg = $"```css\nYour Status: {this}```";
            if (Channel != null)
                await Channel.SendMessageAsync(msg);
        }

        public async Task HitAsync(string by = null)
        {
            if (this.Health > 0)
            {
                this.Health--;

                var msg = "You were hit";
                if (by != null)
                    msg += $" by {by}";

                await SendGameEventWithStateAsync(msg);
            }
        }

        public async Task HealAsync(string by = null)
        {
            if (this.Health < 3)
            {
                this.Health++;

                var msg = "You were healed by ";
                if (by != null)
                    msg += $" by {by}";

                await SendGameEventWithStateAsync(msg);
            }
        }
    }
}
