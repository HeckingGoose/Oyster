using System.Collections.Generic;
using System.Diagnostics;

namespace Oyster.Core.AbstractTypes.Character
{
    public abstract class A_CharacterSprite
    {
        // Protected Variables
        protected A_Sprite[] _sprites;

        // Constructor
        public A_CharacterSprite(List<A_Sprite> sprites) : this(sprites.ToArray()) { }
        public A_CharacterSprite(A_Sprite[] sprites) { _sprites = sprites; }

        // Protected Methods
        /// <summary>
        /// Called when the sprite of this character is requested to be changed.
        /// </summary>
        /// <param name="sprite">An object likely to be the sprite to change to.</param>
        protected abstract void OnSpriteSet(object sprite);

        // Public Methods
        /// <summary>
        /// Given a spritename, attempts to set the sprite of this character.
        /// </summary>
        /// <param name="name">The name of the sprite to change to.</param>
        /// <returns>True on success, false on fail.</returns>
        public bool SetSprite(string name)
        {
            // Cache
            name = name.ToLower();

            // Iterate every sprite
            foreach (A_Sprite sprite in _sprites)
            {
                // Is this the sprite we want?
                if (sprite.Name.ToLower() == name)
                {
                    // Then set and exit
                    OnSpriteSet(sprite.Sprite);
                    return true;
                }
            }

            // Otherwise log and exit
            Debug.WriteLine($"Unable to find sprite '{name}'.");
            return false;
        }
    }
}
