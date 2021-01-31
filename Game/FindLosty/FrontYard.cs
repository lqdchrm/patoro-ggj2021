using DSharpPlus.Entities;
using LostAndFound.Engine;
using LostAndFound.Engine.Attributes;
using LostAndFound.Engine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Game.FindLosty
{
    public class FrontYard : CommonRoom
    {
        public override string Name => "FrontYard";

        public FrontYard() : base()
        {
            Inventory.Add("keys", Emojis.Keys);
        }

        protected override string WhyIsItemNotTakeable(string itemKey)
        {
            return itemKey switch
            {
                "keys" => "better not",
                _ => null
            };
        }

        protected override bool IsCommandVisible(string cmd)
        {
            switch (cmd)
            {
            }
            return base.IsCommandVisible(cmd);
        }
    }
}
