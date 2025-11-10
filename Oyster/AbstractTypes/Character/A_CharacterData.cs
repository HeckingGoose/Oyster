using System.Drawing;

namespace Oyster.AbstractTypes.Character
{
    public abstract class A_CharacterData
    {
        // Protected Variables
        protected string _displayName;
        protected Color _displayNameColour;

        // Constructor
        public A_CharacterData(string name, Color nameColour)
        {
            // Pass Values
            _displayName = name;
            _displayNameColour = nameColour;
        }

        // Public Methods
        /// <summary>
        /// Begins loading the script associated with this character.
        /// </summary>
        /// <returns>An asset loader that will in the future contain the loaded asset.</returns>
        public abstract A_BackgroundAssetLoader<string> BeginScriptLoad();

        // Accessors
        /// <summary>
        /// Gets this character's name.
        /// </summary>
        public string DisplayName { get { return _displayName; } }
        /// <summary>
        /// Gets the colour that this character's name should be drawn as.
        /// </summary>
        public Color DisplayNameColour { get { return _displayNameColour; } }
    }
}
