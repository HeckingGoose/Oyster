namespace Oyster.Core.AbstractTypes.Player
{
    public abstract class A_PlayerTalker
    {
        // Protected Variables
        protected A_SpeechDisplay _speechDisplay;

        // Constructor
        public A_PlayerTalker(A_SpeechDisplay speechDisplay)
        {
            // Pass values
            _speechDisplay = speechDisplay;
        }

        // Accessors
        /// <summary>
        /// Gets the speech display that this player uses.
        /// </summary>
        public A_SpeechDisplay SpeechDisplay { get { return _speechDisplay; } }
    }
}
