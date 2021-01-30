using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LostAndFound.Engine.Attributes;

namespace LostAndFound.Engine
{
    public abstract class BaseRoom
    {
        private readonly Dictionary<string, MethodInfo> Commands = new Dictionary<string, MethodInfo>();
        protected readonly Dictionary<string, CommandAttribute> CommandDefs = new Dictionary<string, CommandAttribute>();

        internal BaseGame Game { get; set; }
        internal DiscordChannel VoiceChannel { get; set; }

        public abstract string Name { get; }

        public BaseRoom()
        {
            BuildCommands();
        }

        public async Task SendMessageAsync(BasePlayer fromPlayer, string msg)
        {
            var tasks = Game.Players.Values
                .Where(p => p.Room == this)
                .Select(player => player.Channel?.SendMessageAsync($"[{fromPlayer.Name}] {msg}"));
            await Task.WhenAll(tasks);
        }

        public async Task SendGameEventAsync(string msg)
        {
            msg = $"```css\n{msg}\n```";
            var tasks = Game.Players.Values
                .Where(p => p.Room == this)
                .Select(player => player.Channel?.SendMessageAsync(msg));
            await Task.WhenAll(tasks);
        }

        public async Task HandleCommandAsync(BasePlayer player, string command)
        {
            MethodInfo method;
            if (Commands.TryGetValue(command, out method))
            {
                var task = (Task)method.Invoke(this, new object[] { player });
                await task;
            }
        }

        private void BuildCommands()
        {
            var methods = GetType().GetMethods().Where(method => method.GetCustomAttributes<CommandAttribute>().Any());
            foreach (var method in methods)
            {
                var attrib = method.GetCustomAttribute<CommandAttribute>();
                Commands.Add(attrib.Name, method);
                CommandDefs.Add(attrib.Name, attrib);
            }
        }
    }
}
