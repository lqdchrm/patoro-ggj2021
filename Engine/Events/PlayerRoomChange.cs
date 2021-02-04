namespace LostAndFound.Engine.Events
{
    public class PlayerRoomChange<TGame, TPlayer, TRoom, TContainer, TThing>
        where TGame : class, BaseGame<TGame, TPlayer, TRoom, TContainer, TThing>
        where TPlayer : class, BasePlayer<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TRoom : class, BaseRoom<TGame, TPlayer, TRoom, TContainer, TThing>, TContainer
        where TContainer : class, BaseContainer<TGame, TPlayer, TRoom, TContainer, TThing>, TThing
        where TThing : class, BaseThing<TGame, TPlayer, TRoom, TContainer, TThing>
    {
        public TPlayer Player { get; internal set; }
        public TRoom OldRoom { get; internal set; }

        public override string ToString()
        {
            return $"{Player}: moved from {OldRoom} to {Player.Room}";
        }
    }
}
