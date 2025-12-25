using Oyster.Core.AbstractTypes;
using Oyster.Core.AbstractTypes.Character;

namespace UnitTests.OysterImplementation.Character
{
    internal class CharacterSprite : A_CharacterSprite
    {
        // Constructor
        public CharacterSprite(A_Sprite[] sprites) : base(sprites)
        {
        }

        // Protected Methods
        protected override void OnSpriteSet(object sprite)
        {
            // Do nothing
        }
    }
}
