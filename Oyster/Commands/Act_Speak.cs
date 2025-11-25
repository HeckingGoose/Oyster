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

        // Constructors
        private Act_Speak(
            string textToDisplay,
            bool instant,
            bool waitForUserInput
            ) : base()
        {
            // Pass in values
            _textToDisplay = textToDisplay;
            _optionalParameters.Add(PARAMETER_INSTANT_NAME, (instant, typeof(bool)));
            _optionalParameters.Add(PARAMETER_WAITFORUSERINPUT_NAME, (waitForUserInput, typeof(bool)));

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

            // Attempt to read in the first value as a string
            LoadParameterValue(rawParameters[0], ref textToDisplay);

            // On fail return null
            if (textToDisplay == null) { return null; }

            // Now read in any optional parameters
            Dictionary<string, (object value, Type type)> optionals = new Dictionary<string, (object value, Type type)>
            {
                { PARAMETER_INSTANT_NAME, (DEFAULT_INSTANT, typeof(bool)) },
                { PARAMETER_WAITFORUSERINPUT_NAME, (DEFAULT_WAITFORUSERINPUT, typeof(bool)) }
            };
            List<string> t = [.. rawParameters];
            t.RemoveAt(0);
            LoadOptionalParameterValues(t.ToArray(), ref optionals);

            // Make and return self
            return new Act_Speak(textToDisplay, (bool)optionals[PARAMETER_INSTANT_NAME].value, (bool)optionals[PARAMETER_WAITFORUSERINPUT_NAME].value);
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
            if ((bool)_optionalParameters[PARAMETER_INSTANT_NAME].value)
            {
                DumpAllRemaining();
            }

            // Are we at the end?
            if (_currentCharacterIndex >= _textToDisplay.Length)
            {
                // Return true if allowed to auto-progress
                return !(bool)_optionalParameters[PARAMETER_WAITFORUSERINPUT_NAME].value;
            }

            // Are we ready for the next character?
            if (_timer > OysterMain.CharacterTalker!.Data.TimeBetweenCharacters)
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
            _timer += Definitions.TICKRATE_WAITTIME;

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
                _optionalParameters[PARAMETER_WAITFORUSERINPUT_NAME] = (false, typeof(bool));
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