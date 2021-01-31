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
        #region Commands
        
        private readonly Dictionary<string, MethodInfo> CommandMethods = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, CommandAttribute> CommandDefs = new Dictionary<string, CommandAttribute>();
        
        protected IEnumerable<CommandAttribute> Commands => CommandDefs.Values.Where(cmd => IsCommandVisible(cmd.Name));
        
        protected abstract bool IsCommandVisible(string cmdName);
        
        internal void HandleCommand(PlayerCommand cmd)
        {
            MethodInfo method;
            if (IsCommandVisible(cmd.Command) && CommandMethods.TryGetValue(cmd.Command, out method))
            {
                method.Invoke(this, new object[] { cmd });
            }
            else
            {
                foreach (var player in Players)
                {
                    player.Channel?.SendMessageAsync($"[{player}] {cmd.Message}");
                }
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
        #endregion

        internal DiscordChannel VoiceChannel { get; set; }
        protected internal virtual BaseGame Game { get; set; }

        public abstract string Name { get; }

        public IEnumerable<BasePlayer> Players => Game.Players.Values.Where(p => p.Room == this).ToList();

        #region Visibility
        public bool IsVisible { get; set; }

        public Task Show() => Game.SetRoomVisibility(this, true);
        public Task Hide() => Game.SetRoomVisibility(this, false);
        #endregion

        public BaseRoom()
        {
            BuildCommands();
        }

        public void SendGameEvent(string msg, BasePlayer excludePlayer = null)
        {
            msg = $"```css\n{msg}\n```";
            foreach(var player in Players.Where(p => excludePlayer != p))
            {
                player.Channel?.SendMessageAsync(msg);
            }
        }
    }
}
