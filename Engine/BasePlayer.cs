using DSharpPlus.Entities;
using System.Threading.Tasks;
using System.Linq;
using DSharpPlus;

namespace LostAndFound.Engine
{
    public abstract class BasePlayer<TGame, TRoom, TPlayer, TThing> : BaseContainer<TGame, TRoom, TPlayer, TThing>
        where TGame : BaseGame<TGame, TRoom, TPlayer, TThing>
        where TRoom : BaseRoom<TGame, TRoom, TPlayer, TThing>
        where TPlayer : BasePlayer<TGame, TRoom, TPlayer, TThing>
        where TThing : BaseThing<TGame, TRoom, TPlayer, TThing>
    {
        private DiscordChannel _Channel;
        private DiscordMember _Member { get; set; }

        public TRoom Room { get; set; }
        public string NormalizedName => string.Join("", Name.ToLowerInvariant().Where(c => (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')));

        public override string Emoji => emoji;


        public BasePlayer(TGame game, DiscordMember member) : base(game, false, false, member.DisplayName)
        {
            emoji = Emojis.Players.TakeOneRandom();
            WasMentioned = true;
            this._Member = member;
        }

        internal async Task _InitPlayer()
        {
            var builder = new DiscordOverwriteBuilder();
            var guild = _Member.Guild;
            if (guild != null)
            {
                var discordOverwriteBuilder = builder.For(guild.EveryoneRole).Deny(Permissions.AccessChannels);
                var overwrites = new[] { discordOverwriteBuilder };
                var channel = await guild.CreateChannelAsync($"{this.Emoji}{this.Name}", ChannelType.Text, Game._ParentChannel, overwrites: overwrites);
                await channel.AddOverwriteAsync(_Member, Permissions.AccessChannels);
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

        public virtual string StatusText => this.ToString();

        private BaseThing<TGame, TRoom, TPlayer, TThing> thingPlayerIsUsingAndHasToStop;
        public BaseThing<TGame, TRoom, TPlayer, TThing> ThingPlayerIsUsingAndHasToStop
        {
            get
            {
                return thingPlayerIsUsingAndHasToStop;
            }
            set
            {
                if (this is TPlayer self)
                {
                    if (thingPlayerIsUsingAndHasToStop != null && value == null)
                    {
                        this.Room.SendText($"{this} stopped using {thingPlayerIsUsingAndHasToStop}", self);
                    }
                    else if (value != null && thingPlayerIsUsingAndHasToStop != value)
                    {
                        this.Room.SendText($"{this} started using {value}", self);
                    }
                    thingPlayerIsUsingAndHasToStop = value;
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
        public override string LookInventoryText => "\nBackpack:\n\t" + string.Join("\n\t", Inventory.Select(i => i.ToString()));
        public virtual string LookStatus => this.StatusText;
        public override string LookText => $"{LookTextHeader}{LookInventoryText}\n{LookStatus}";

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
        public void Mute()
        {
            _Member?.ModifyAsync(x => x.Muted = true);
        }

        public void Unmute()
        {
            _Member?.ModifyAsync(x => x.Muted = false);
        }

        public bool Reply(string msg)
        {
            msg = $"```css\n{msg}```";
            _Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplyWithState(string msg)
        {
            msg = $"```css\n{msg}\nYour Status: {StatusText}```";
            _Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplyImage(string msg)
        {
            msg = $"```\n{msg}\n```";
            _Channel?.SendMessageAsync(msg);
            return true;
        }

        public bool ReplySpeach(string msg)
        {
            _Channel?.SendMessageAsync(msg, true);
            return true;
        }



        internal bool _UsesChannel(DiscordChannel channel) => channel == this._Channel && channel is not null;

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
