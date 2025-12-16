using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Act_Append : A_Command, ITakesTime, ITextPusher
    {
        // Const
        protected const bool DEFAULT_INSTANT = false;
        protected const bool DEFAULT_WAITFORUSERINPUT = true;
        protected const bool DEFUALT_MUTE = false;
        protected const int START_POS = 0;

        protected const string PARAMETER_INSTANT_NAME = "instant";
        protected const string PARAMETER_WAITFORUSERINPUT_NAME = "wait";
        protected const string PARAMETER_MUTE_NAME = "mute";


        // Private Variables
        protected string _textToDisplay;
        protected float _timer;
        protected int _currentCharacterIndex;

        // Constructors
        protected Act_Append(
            string textToDisplay,
            bool instant,
            bool waitForUserInput,
            bool mute
            ) : base()
        {
            // Pass in values
            _textToDisplay = textToDisplay;
            _optionalParameters.Add(PARAMETER_INSTANT_NAME, (instant, typeof(bool)));
            _optionalParameters.Add(PARAMETER_WAITFORUSERINPUT_NAME, (waitForUserInput, typeof(bool)));
            _optionalParameters.Add(PARAMETER_MUTE_NAME, (mute, typeof(bool)));

            // Default
            _timer = START_POS;
            _currentCharacterIndex = START_POS;
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
                { PARAMETER_WAITFORUSERINPUT_NAME, (DEFAULT_WAITFORUSERINPUT, typeof(bool)) },
                { PARAMETER_MUTE_NAME, (DEFUALT_MUTE, typeof(bool)) }
            };
            List<string> t = [.. rawParameters];
            t.RemoveAt(0);
            LoadOptionalParameterValues(t.ToArray(), ref optionals);

            // Make and return self
            return new Act_Append(
                textToDisplay,
                (bool)optionals[PARAMETER_INSTANT_NAME].value,
                (bool)optionals[PARAMETER_WAITFORUSERINPUT_NAME].value,
                (bool)optionals[PARAMETER_MUTE_NAME].value
                );
        }
        public override bool Run()
        {
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
                _timer -= OysterMain.CharacterTalker!.Data.TimeBetweenCharacters;

                // Get a character and push it
                string toAdd = ParseForRTT(_textToDisplay, _currentCharacterIndex);
                OysterMain.PlayerTalker!.SpeechDisplay.MainText.Text += toAdd;

                // Increment counter by this length
                _currentCharacterIndex += toAdd.Length;
            }

            // TODO: Figure out how audio should be implemented

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
        /// <summary>
        /// Returns the text that this command will display.
        /// </summary>
        public string TextToDisplay { get { return _textToDisplay; } }
        /// <summary>
        /// Returns whether this command will complete within one tick.
        /// </summary>
        public bool Instant { get { return (bool)_optionalParameters[PARAMETER_INSTANT_NAME].value; } }
        /// <summary>
        /// Returns whether this command will wait for user input before going to the next command.
        /// </summary>
        public bool WaitForUserInput { get { return (bool)_optionalParameters[PARAMETER_WAITFORUSERINPUT_NAME].value; } }
        /// <summary>
        /// Returns whether this command will produce speech sounds or not.
        /// </summary>
        public bool Mute { get { return (bool)_optionalParameters[PARAMETER_MUTE_NAME].value; } }
    }
}