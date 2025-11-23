using Oyster.Core.AbstractTypes;
using Oyster.Core.AbstractTypes.Character;
using Oyster.Core.AbstractTypes.Player;
using Oyster.Core.AbstractTypes.Scene;
using System.Diagnostics;

namespace Oyster.Core
{
    public static class Oyster
    {
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
        // < Conversation Loading >
        private static A_BackgroundAssetLoader<string>? _scriptLoader;
        private static string? _rawScript;

        // Constructor
        static Oyster()
        {
            // Do nothing
        }

        // Private Methods
        /// <summary>
        /// Called when _scriptLoader finishes.
        /// </summary>
        private static void OnScriptLoaded(A_BackgroundAssetLoader<string>.LoadResult loadResult, string log)
        {
            // Did it load successfully?
            switch (loadResult)
            {
                // Yes
                case A_BackgroundAssetLoader<string>.LoadResult.Succeeded:
                    // Cache raw script
                    _rawScript = _scriptLoader!.Asset;

                    // Begin converting to a conversation.
                    break;

                // No
                case A_BackgroundAssetLoader<string>.LoadResult.Failed:
                default:
                    // Then log issue and end conversation
                    Debug.WriteLine(log);
                    EndChat();
                    break;
            }

            // Now that we're done we should clean up the script loading stuff
            _scriptLoader = null;
        }
        /// <summary>
        /// Cleans up after a conversation.
        /// </summary>
        private static void EndChat()
        {
            // Tell the player's speech display to hide itself
            if (_playerScript != null) _playerScript.SpeechDisplay.Hide();

            // And now tell the scene script to reset itself
            if (_sceneScript != null) _sceneScript.ShowObjectsPostChat();

            // Now null everything out
            _playerScript = null;
            _characterScript = null;
            _sceneScript = null;
            _rawScript = null;
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

            // Begin loading the script for this conversation
            _scriptLoader = _characterScript.Data.BeginScriptLoad();
            _scriptLoader.BeginAssetLoad();
            Debug.WriteLine("Loading script for conversation.");

            _scriptLoader.OnLoadFinished += OnScriptLoaded;
            return true;
        }

        // Accessors
        public static A_SceneScript? SceneScript { get { return _sceneScript; } set { _sceneScript = value; } }
    }
}
