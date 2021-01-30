using System.Collections.Generic;

namespace LostAndFound.Game.Mansion.Characters
{
    internal abstract class BaseCharacter
    {
        public readonly HashSet<BaseCharacter> CharactersSeen = new HashSet<BaseCharacter>();
        public readonly HashSet<BaseCharacter> RoomsSeen = new HashSet<BaseCharacter>();

        public MansionPlayer Player { get; set; }
        public abstract string[] Prolog { get; }

        public abstract string StartLocation { get; }

    }
}
