namespace Oyster.AbstractTypes.Character
{
    public abstract class A_CharacterTalker
    {
        // Protected Variables
        protected A_CharacterData _data;

        // Constructor
        public A_CharacterTalker(A_CharacterData data)
        {
            // Pass Values
            _data = data;
        }

        // Accessors
        /// <summary>
        /// Gets the object representing this character's configuration.
        /// </summary>
        public A_CharacterData Data { get { return _data; } }
    }
}
