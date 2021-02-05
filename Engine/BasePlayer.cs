using DSharpPlus;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Engine
{

    public interface BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        TRoom Room { get; set; }
        string NormalizedName { get; }

        string StatusText { get; }

        public TThing ThingPlayerIsUsingAndHasToStop { get; set; }

        void Mute();

        void Unmute();

        bool Reply(string msg);

        bool ReplyWithState(string msg);

        bool ReplyImage(string msg);

        bool ReplySpeach(string msg);

        public void MoveTo(TRoom room);

#pragma warning disable IDE1006 // Naming Styles

        Task _InitPlayer(DiscordChannel parentChannel);
        bool _UsesChannel(DiscordChannel channel);

#pragma warning restore IDE1006 // Naming Styles
    }

    public abstract class BasePlayerImpl<TGame, TPlayer, TRoom, TContainer, TThing>
        : BaseContainerImpl<TGame, TPlayer, TRoom, TContainer, TThing>, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
#pragma warning disable IDE1006 // Naming Styles

        private DiscordChannel _Channel;
        private DiscordMember _Member { get; set; }

#pragma warning restore IDE1006 // Naming Styles


        public TRoom Room { get; set; }
        public string NormalizedName => string.Join("", this.Name.ToLowerInvariant().Where(c => (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')));

        public override string Emoji => this.emoji;

        public BasePlayerImpl(TGame game, DiscordMember member) : base(game, false, false, member.DisplayName)
        {
            this.emoji = Emojis.Players.TakeOneRandom();
            this.WasMentioned = true;
            this._Member = member;
        }

        public async Task _InitPlayer(DiscordChannel parentChannel)
        {
            var builder = new DiscordOverwriteBuilder();
            var guild = this._Member.Guild;
            if (guild != null)
            {
                var discordOverwriteBuilder = builder.For(guild.EveryoneRole).Deny(Permissions.AccessChannels);
                var overwrites = new[] { discordOverwriteBuilder };
                var channel = await guild.CreateChannelAsync($"{this.Emoji}{this.Name}", ChannelType.Text, parentChannel, overwrites: overwrites);
                await channel.AddOverwriteAsync(this._Member, Permissions.AccessChannels);
                this._Channel = channel;
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
        private readonly string emoji;

        public virtual string StatusText => ToString();

        private TThing thingPlayerIsUsingAndHasToStop;
        public TThing ThingPlayerIsUsingAndHasToStop
        {
            get => this.thingPlayerIsUsingAndHasToStop;
            set
            {
                if (this is TPlayer self)
                {
                    if (this.thingPlayerIsUsingAndHasToStop != null && value == null)
                    {
                        this.Room.SendText($"{this} stopped using {this.thingPlayerIsUsingAndHasToStop}", self);
                    }
                    else if (value != null && this.thingPlayerIsUsingAndHasToStop != value)
                    {
                        this.Room.SendText($"{this} started using {value}", self);
                    }
                    this.thingPlayerIsUsingAndHasToStop = value;
                }
            }
        }

        /*
        ██╗      ██████╗  ██████╗ ██╗  ██╗
        ██║     ██╔═══██╗██╔═══██╗██║ ██╔╝
        ██║     ██║   ██║██║   ██║█████╔╝ 
        ██║     ██║   ██║██║   ██║██╔═██╗ 
        ███████╗╚██████╔╝╚██████╔╝██║  ██╗
        ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
        */
        public override string LookInventoryText => "\nBackpack:\n\t" + string.Join("\n\t", this.Inventory.Select(i => i.ToString()));
        public virtual string LookStatus => this.StatusText;
        public override string LookText => $"{this.LookTextHeader}{this.LookInventoryText}\n{this.LookStatus}";

        /*
        ██╗  ██╗██╗ ██████╗██╗  ██╗
        ██║ ██╔╝██║██╔════╝██║ ██╔╝
        █████╔╝ ██║██║     █████╔╝ 
        ██╔═██╗ ██║██║     ██╔═██╗ 
        ██║  ██╗██║╚██████╗██║  ██╗
        ╚═╝  ╚═╝╚═╝ ╚═════╝╚═╝  ╚═╝
        */


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
        public void Mute() => this._Member?.ModifyAsync(x => x.Muted = true);

        public void Unmute() => this._Member?.ModifyAsync(x => x.Muted = false);

        public bool Reply(string msg)
        {
            msg = $"```css\n{msg}```";
            this._Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplyWithState(string msg)
        {
            msg = $"```css\n{msg}\nYour Status: {this.StatusText}```";
            this._Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplyImage(string msg)
        {
            msg = $"```\n{msg}\n```";
            this._Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplySpeach(string msg)
        {
            this._Channel?.SendMessageAsync(msg, true);
            return true;
        }

        public void MoveTo(TRoom room)
        {
            if (room is not BaseRoomImpl<TGame, TPlayer, TRoom, TContainer, TThing> romImp)
                return;
            romImp._VoiceChannel.PlaceMemberAsync(this._Member);
        }

        public bool _UsesChannel(DiscordChannel channel) => channel == this._Channel && channel is not null;

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
