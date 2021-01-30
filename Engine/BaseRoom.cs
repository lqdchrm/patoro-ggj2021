using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;

namespace LostAndFound.Engine
{
    public abstract class BaseRoom
    {
        private readonly Dictionary<string, MethodInfo> CommandMethods = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, CommandAttribute> CommandDefs = new Dictionary<string, CommandAttribute>();

        protected IEnumerable<CommandAttribute> Commands => CommandDefs.Values.Where(cmd => IsCommandVisible(cmd.Name));
        protected abstract bool IsCommandVisible(string cmdName);

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
                .Select(player => player.Channel?.SendMessageAsync($"[{fromPlayer}] {msg}"));
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

        public async Task HandleCommandAsync(PlayerCommand cmd)
        {
            MethodInfo method;
            if (IsCommandVisible(cmd.Command) &&  CommandMethods.TryGetValue(cmd.Command, out method))
            {
                var task = (Task)method.Invoke(this, new object[] { cmd });
                await task;
            } else
            {
                await cmd.Player.Room.SendMessageAsync(cmd.Player, cmd.Message);
            }
        }

        private void BuildCommands()
        {
            var methods = GetType().GetMethods().Where(method => method.GetCustomAttributes<CommandAttribute>().Any());
            foreach (var method in methods)
            {
                var attrib = method.GetCustomAttribute<CommandAttribute>();
                CommandMethods.Add(attrib.Name, method);
                CommandDefs.Add(attrib.Name, attrib);
            }
        }
    }
}
