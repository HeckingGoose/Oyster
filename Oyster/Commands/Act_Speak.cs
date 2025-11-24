using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Act_Speak : A_Command, ITakesTime, ITextPusher
    {
        // Const
        private const bool DEFAULT_INSTANT = false;
        private const bool DEFAULT_WAITFORUSERINPUT = true;
        private const int START_POS = 0;

        private const string PARAMETER_INSTANT_NAME = "instant";
        private const string PARAMETER_WAITFORUSERINPUT_NAME = "wait";


        // Private Variables
        private string _textToDisplay;
        private float _timer;
        private int _currentCharacterIndex;
        private bool _instant;
        private bool _waitForUserInput;

        // Constructors
        private Act_Speak(
            string textToDisplay,
            bool instant,
            bool waitForUserInput
            )
        {
            // Pass in values
            _textToDisplay = textToDisplay;
            _instant = instant;
            _waitForUserInput = waitForUserInput;

            // Default
            _timer = START_POS;
            _currentCharacterIndex = START_POS;
        }

        // Explicit Interface Implementations
        public override ISpeechCommand CreateSelf(string[] rawParameters)
        {
            return CreateSelf(rawParameters);
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Declare stores
            string? textToDisplay = string.Empty;
            bool instant = DEFAULT_INSTANT;
            bool waitForUserInput = DEFAULT_WAITFORUSERINPUT;

            // Attempt to read in the first value as a string
            LoadParameterValue(rawParameters[0], ref textToDisplay);

            // On fail return null
            if (textToDisplay == null) { return null; }

            // Now let's look through every other parameter that was given
            for (int i = 1; i < rawParameters.Length; i++)
            {
                // Split across the splitter
                string[] split = SplitIntoVariableAndData(rawParameters[i]);

                // Let's see what we've got
                switch (split[0].ToLower())
                {
                    // This is referring to instant
                    case PARAMETER_INSTANT_NAME:
                        // Read in the bool
                        LoadParameterValue(split[i], ref instant);
                        break;

                    // This is referring to the waiting
                    case PARAMETER_WAITFORUSERINPUT_NAME:
                        // Read it in
                        LoadParameterValue(split[1], ref waitForUserInput);
                        break;
                }
            }

            // Make and return self
            return new Act_Speak(textToDisplay, instant, waitForUserInput);
        }
        public override bool Run()
        {

            // Is this the first run?
            if (_currentCharacterIndex == START_POS)
            {
                // Clear text box
                OysterMain.PlayerTalker!.SpeechDisplay.MainText.Text = string.Empty;
            }

            // Is this text intended to be instant?
            if (_instant)
            {
                DumpAllRemaining();
            }

            // Are we at the end?
            if (_currentCharacterIndex >= _textToDisplay.Length)
            {
                // Return true if allowed to auto-progress
                return !_waitForUserInput;
            }

            // Are we ready for the next character?
            if (_timer > speechSystem.NPC.CharacterData.TimeBetweenCharacters)
            {
                // Reset it
                _timer = START_POS;

                // Get a character and push it
                string toAdd = ReadIn.ParseForRTT(_textToDisplay, _currentCharacterIndex);
                OysterMain.PlayerTalker!.SpeechDisplay.MainText.Text += toAdd;

                // Increment counter by this length
                _currentCharacterIndex += toAdd.Length;

                // Play back a sound if it was a single character and we have a sounder
                if (speechSystem.NPC.SoundPlayer != null &&
                    toAdd.Length == 1)
                {
                    speechSystem.NPC.SoundPlayer.Play();
                }
            }

            // Increment timer
            _timer += Time.deltaTime;

            // Return that we're not done yet
            return false;
        }

        public void MakeItGoFaster()
        {
            // Are we not at the end?
            if (HasCharactersRemaining)
            {
                DumpAllRemaining();
            }

            // Otherwise
            else
            {
                // Time to skip! To do this we just tell ourself that we no longer care for user input
                _waitForUserInput = false;
            }
        }

        // Private Methods
        private void DumpAllRemaining()
        {
            // Then push all remaining characters
            OysterMain.PlayerTalker!.SpeechDisplay.MainText.Text += _textToDisplay.Substring(_currentCharacterIndex);

            // And update position accordingly
            _currentCharacterIndex = _textToDisplay.Length;
        }

        // Accessors
        public bool HasCharactersRemaining
        {
            get
            {
                // Return if there are any characters left
                return _currentCharacterIndex < _textToDisplay.Length;
            }
        }
    }
}