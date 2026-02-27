using Oyster.Core.AbstractTypes.Scene;

namespace Oyster.Core.AbstractTypes.Character
{
    public abstract class A_CharacterTalker
    {
        // Protected Variables
        protected A_CharacterData _data;
        protected A_CharacterSprite _spriteManager;
        protected A_Looker _looker;

        // Constructor
        public A_CharacterTalker(A_CharacterData data, A_CharacterSprite spriteManager, A_Looker looker)
        {
            // Pass Values
            _data = data;
            _spriteManager = spriteManager;
            _looker = looker;
        }

        // Accessors
        /// <summary>
        /// Gets the object representing this character's configuration.
        /// </summary>
        public A_CharacterData Data { get { return _data; } }
        /// <summary>
        /// Gets the sprite manager for this character.
        /// </summary>
        public A_CharacterSprite SpriteManager { get { return _spriteManager; } }
        /// <summary>
        /// Gets a reference to this NPC's look target.
        /// </summary>
        public A_Looker NPCLooker { get { return _looker; } }
    }
}
