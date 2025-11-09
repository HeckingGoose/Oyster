using Oyster.AbstractTypes;
using Oyster.AbstractTypes.Character;
using System.Diagnostics;

namespace Oyster
{
    public static class Oyster
    {
        // Const
        private const char LINE_ENDING = '\n';
        private const char COMMAND_DATA_SPLITTER = ' ';
        private const char DATA_START_CHAR = '[';
        private const char DATA_END_CHAR = ']';
        private const char COMMENT_CHAR = '#';
        private const string BAD_LINE_ENDING = "\r";
        private const int EXPECTED_SPLIT_SIZE = 2;
        private const char PARAMETER_SEPERATOR = ',';
        private const int PROMPT_WAIT_TIME = 4;

        // Enum
        public enum SpeechState
        {
            NotTalking,
            Loading,
            Talking
        }

        // Delegates
        public delegate void PuppetCall(string command);
        public delegate void BlankDelegate();

        // Events
        public static event PuppetCall? OnPuppetCalled;
        public static event BlankDelegate? OnSpeechCompleted;
        public static event BlankDelegate? OnScriptGenerated;

        // Private Variables
        // < Scene Objects >
        private static A_SceneScript? _sceneScript;
        private static A_PlayerTalker? _playerScript;
        private static A_CharacterTalker? _characterScript;

        // Constructor
        static Oyster()
        {
            // Do nothing
        }

        // Public Methods
        /// <summary>
        /// Attempts to start a chat with the given parameters.
        /// </summary>
        /// <param name="sceneScript">The script for the game's current scene.</param>
        /// <param name="playerTalker">The player's conversation script.</param>
        /// <param name="characterTalker">The character's conversation script.</param>
        /// <returns>True if a conversation is set to be loaded, false if anything went wrong.</returns>
        public static bool StartChat(
            A_SceneScript sceneScript,
            A_PlayerTalker playerTalker,
            A_CharacterTalker characterTalker
            )
        {
            // Ensure these are actual things
            if (sceneScript == null || playerTalker == null || characterTalker == null) { Debug.WriteLine("At least one parameter to StartChat() was null."); return false; }

            // Pass these values across for later use
            _sceneScript = sceneScript;
            _playerScript = playerTalker;
            _characterScript = characterTalker;
            Debug.WriteLine("Cached all relevant scripts for conversation.");

            // TODO: Figure out how to implement the 'lookers'

            // Tell the scene script to hide anything it needs to
            _sceneScript.HideObjectsForChat();

            // Tell the player script to reset its display
            _playerScript.SpeechDisplay.ResetDisplay();

            // And now we should update the display using the character info
            _playerScript.SpeechDisplay.NameText.Text = _characterScript.Data.DisplayName;
            _playerScript.SpeechDisplay.NameText.TextColour = _characterScript.Data.DisplayNameColour;
            Debug.WriteLine("Finished prepping speech display.");

            // Now let's show the speech box since it's all set-up
            _playerScript.SpeechDisplay.Show();
            Debug.WriteLine("Told the player to show their speech display.");

            // TODO: Tell the thingy to start loading. This should not be IMPLEMENTED here though, as it's up to the game to manage file loading.
        }

        // Accessors
        public static A_SceneScript? SceneScript { get { return _sceneScript; } set { _sceneScript = value; } }
    }
}
