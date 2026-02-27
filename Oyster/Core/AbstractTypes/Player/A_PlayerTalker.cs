namespace Oyster.Core.AbstractTypes.Player
{
    public abstract class A_PlayerTalker
    {
        // Protected Variables
        protected A_Camera _camera;
        protected A_SpeechDisplay _speechDisplay;

        // Constructor
        public A_PlayerTalker(
            A_Camera camera,
            A_SpeechDisplay speechDisplay
            )
        {
            // Pass values
            _camera = camera;
            _speechDisplay = speechDisplay;
        }

        // Accessors
        /// <summary>
        /// Gets a reference to the player's camera.
        /// </summary>
        public A_Camera Camera { get { return _camera; } }
        /// <summary>
        /// Gets the speech display that this player uses.
        /// </summary>
        public A_SpeechDisplay SpeechDisplay { get { return _speechDisplay; } }
    }
}
