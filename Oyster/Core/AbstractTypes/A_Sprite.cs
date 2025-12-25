namespace Oyster.Core.AbstractTypes
{
    public abstract class A_Sprite
    {
        // Accessors
        /// <summary>
        /// Returns the name of this sprite.
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Returns an object that should be the sprite associated with this sprite.
        /// </summary>
        public abstract object Sprite { get; }
    }
}
