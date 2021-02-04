using LostAndFound.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.FindLosty
{
    public interface IThing : BaseThing<IFindLostyGame, IPlayer, IRoom, IContainer, IThing> { }
    public abstract class Thing : BaseThingImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing
    {
        public Thing(FindLostyGame game) : this(game, false, null) { }
        public Thing(FindLostyGame game, bool transferable, string name) : base(game, transferable, name) { }
    }

    public interface IContainer : BaseContainer<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IThing { }
    public abstract class Container : BaseContainerImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer
    {
        public Container(FindLostyGame game) : this(game, null) { }
        public Container(FindLostyGame game, bool transferable, string name) : base(game, false, false, name) { }
        public Container(FindLostyGame game, string name) : base(game, true, false, name) { }
    }


    public interface IRoom : BaseRoom<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IContainer { }
    public abstract class Room : BaseRoomImpl<IFindLostyGame, IPlayer, IRoom, IContainer, IThing>, IRoom {
        public Room(FindLostyGame game) : this(game, null) { }
        public Room(FindLostyGame game, string name) : base(game, name) { }
    }


    public abstract class Item : Thing {
        public Item(FindLostyGame game) : this(game, null) { }
        public Item(FindLostyGame game, string name) : base(game, true, name) { }
    }
}
