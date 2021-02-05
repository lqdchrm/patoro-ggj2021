using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{
    public interface BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public string RoomNumber { get; }
        public IEnumerable<TPlayer> Players { get; }

        public void SendText(string msg, IEnumerable<TPlayer> excludedPlayers);
        public void SendText(string msg, params TPlayer[] excludedPlayers);

        public void Say(string msg, IEnumerable<TPlayer> excludedPlayers);
        public void Say(string msg, params TPlayer[] excludedPlayers);

        public Task Show(bool silent = false);

        public Task Hide(bool silent = false);
    }

    public abstract class BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseContainerImpl<TGame, TPlayer, TRoom, TContainer, TThing>, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        internal DiscordChannel _VoiceChannel { get; set; }

        public string RoomNumber { get; init; }

        public BaseRoomImpl(TGame game, string roomNumber, string name = null) : base(game, false, true, name)
        {
            this.RoomNumber = roomNumber;
            this.WasMentioned = true;
        }

        public void SendText(string msg, params TPlayer[] excludedPlayers) => SendText(msg, excludedPlayers as IEnumerable<TPlayer>);
        public void SendText(string msg, IEnumerable<TPlayer> excludedPlayers)
        {
            Send(msg, false, excludedPlayers);
        }

        public void Say(string msg, params TPlayer[] excludedPlayers) => Say(msg, excludedPlayers as IEnumerable<TPlayer>);
        public void Say(string msg, IEnumerable<TPlayer> excludedPlayers)
        {
            msg = $"{msg}";
            Send(msg, true, excludedPlayers);
        }

        private void Send(string msg, bool tts, IEnumerable<TPlayer> excludedPlayers)
        {
            foreach (var player in this.Players.Where(p => !excludedPlayers.Contains(p)))
            {
                if (tts)
                    player.ReplySpeach(msg);
                else
                    player.Reply(msg);
            }
        }

        /*
        ███████╗████████╗ █████╗ ████████╗███████╗
        ██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
        ███████╗   ██║   ███████║   ██║   █████╗  
        ╚════██║   ██║   ██╔══██║   ██║   ██╔══╝  
        ███████║   ██║   ██║  ██║   ██║   ███████╗
        ╚══════╝   ╚═╝   ╚═╝  ╚═╝   ╚═╝   ╚══════╝
        */
        public IEnumerable<TPlayer> Players => this.Game.Players.Values.Where(p => this.Equals(p.Room)).ToList();

        public Task Show(bool silent = false)
        {
            if (!this.IsVisible)
            {
                var role = this._VoiceChannel?.Guild.EveryoneRole;
                if (role is not null)
                {
                    this.IsVisible = true;

                    if (!silent)
                        this.Game.Say($"The new Room {this.Name} has appeared. You can switch Voice channels now.");

                    return this._VoiceChannel.AddOverwriteAsync(role, allow: Permissions.AccessChannels);
                }
            }
            return Task.CompletedTask;
        }

        public Task Hide(bool silent = false)
        {
            if (this.IsVisible)
            {
                var role = this._VoiceChannel?.Guild.EveryoneRole;
                if (role is not null)
                {
                    this.IsVisible = false;
                    return this._VoiceChannel.AddOverwriteAsync(role, deny: Permissions.AccessChannels);
                }
            }
            return Task.CompletedTask;
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public virtual string LookIntroText(TPlayer sender) => $"You are at {this}.";
        public override string LookInventoryText => string.Join(", ", this.Inventory.Where(i => i.CanBeTransfered));

        public override void Look(TPlayer sender)
        {
            var intro = LookIntroText(sender);
            var content = this.LookInventoryText;

            sender.ReplyWithState($"{intro}\n{content}\n");
        }

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */
        public override string KickText => OneOf($"You kicked into thin air.");

        /*
        ██╗     ██╗███████╗████████╗███████╗███╗   ██╗
        ██║     ██║██╔════╝╚══██╔══╝██╔════╝████╗  ██║
        ██║     ██║███████╗   ██║   █████╗  ██╔██╗ ██║
        ██║     ██║╚════██║   ██║   ██╔══╝  ██║╚██╗██║
        ███████╗██║███████║   ██║   ███████╗██║ ╚████║
        ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝
        */

        /*
         ██████╗ ██████╗ ███████╗███╗   ██╗
        ██╔═══██╗██╔══██╗██╔════╝████╗  ██║
        ██║   ██║██████╔╝█████╗  ██╔██╗ ██║
        ██║   ██║██╔═══╝ ██╔══╝  ██║╚██╗██║
        ╚██████╔╝██║     ███████╗██║ ╚████║
         ╚═════╝ ╚═╝     ╚══════╝╚═╝  ╚═══╝
        */

        /*
         ██████╗██╗      ██████╗ ███████╗███████╗
        ██╔════╝██║     ██╔═══██╗██╔════╝██╔════╝
        ██║     ██║     ██║   ██║███████╗█████╗  
        ██║     ██║     ██║   ██║╚════██║██╔══╝  
        ╚██████╗███████╗╚██████╔╝███████║███████╗
         ╚═════╝╚══════╝ ╚═════╝ ╚══════╝╚══════╝
        */

        /*
        ████████╗ █████╗ ██╗  ██╗███████╗
        ╚══██╔══╝██╔══██╗██║ ██╔╝██╔════╝
           ██║   ███████║█████╔╝ █████╗  
           ██║   ██╔══██║██╔═██╗ ██╔══╝  
           ██║   ██║  ██║██║  ██╗███████╗
           ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
        */

        /*
        ██████╗ ██╗   ██╗████████╗
        ██╔══██╗██║   ██║╚══██╔══╝
        ██████╔╝██║   ██║   ██║   
        ██╔═══╝ ██║   ██║   ██║   
        ██║     ╚██████╔╝   ██║   
        ╚═╝      ╚═════╝    ╚═╝   
        */

        /*
        ██╗   ██╗███████╗███████╗
        ██║   ██║██╔════╝██╔════╝
        ██║   ██║███████╗█████╗  
        ██║   ██║╚════██║██╔══╝  
        ╚██████╔╝███████║███████╗
         ╚═════╝ ╚══════╝╚══════╝
        */

        /*
        ██╗  ██╗███████╗██╗     ██████╗ ███████╗██████╗ ███████╗
        ██║  ██║██╔════╝██║     ██╔══██╗██╔════╝██╔══██╗██╔════╝
        ███████║█████╗  ██║     ██████╔╝█████╗  ██████╔╝███████╗
        ██╔══██║██╔══╝  ██║     ██╔═══╝ ██╔══╝  ██╔══██╗╚════██║
        ██║  ██║███████╗███████╗██║     ███████╗██║  ██║███████║
        ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝
        */
    }
}
