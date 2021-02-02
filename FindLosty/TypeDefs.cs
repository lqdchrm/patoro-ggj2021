using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public abstract class Room : BaseRoom<FindLostyGame, Room, Player, Thing> { public Room(FindLostyGame game, string name = null) : base(game, name) { } }
    public abstract class Thing : BaseThing<FindLostyGame, Room, Player, Thing> { public Thing(FindLostyGame game, string name = null) : base(game, false, name) { } }
    public abstract class Item : BaseThing<FindLostyGame, Room, Player, Thing> { public Item(FindLostyGame game, string name = null) : base(game, true, name) { } }
    public abstract class Container : BaseContainer<FindLostyGame, Room, Player, Thing> { public Container(FindLostyGame game, string name = null) : base(game, true, false, name) { } }

}
