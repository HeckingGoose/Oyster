using System.Drawing;

namespace Oyster.Core.AbstractTypes.Character
{
    public abstract class A_CharacterData
    {
        // Protected Variables
        protected string _displayName;
        protected Color _displayNameColour;
        protected float _timeBetweenCharacters;

        // Constructor
        public A_CharacterData(string name, Color nameColour, float timeBetweenCharacters)
        {
            // Pass Values
            _displayName = name;
            _displayNameColour = nameColour;
            _timeBetweenCharacters = timeBetweenCharacters;
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
        /// <summary>
        /// Gets the time that should occur between each character being pushed to the display for this character.
        /// </summary>
        public float TimeBetweenCharacters { get { return _timeBetweenCharacters; } }
    }
}
