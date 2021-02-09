using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patoro.TAE.Discord
{
    public class DiscordGameRegistry : Dictionary<string, Func<string, DiscordClient, DiscordGuild, IGame>>
    {
    }
}
