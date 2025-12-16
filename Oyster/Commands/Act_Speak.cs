using Oyster.Core;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Act_Speak : Act_Append
    {
        // Constructors
        private Act_Speak(
            string textToDisplay,
            bool instant,
            bool waitForUserInput,
            bool mute
            ) : base(textToDisplay, instant, waitForUserInput, mute) { }

        // Explicit Interface Implementations
        public static new ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Make a thing
            Act_Append a = (Act_Append)Act_Append.MakeSelf(rawParameters)!;

            // Now read its values
            return new Act_Speak(a.TextToDisplay, a.Instant, a.WaitForUserInput, a.Mute);
        }
        public override bool Run()
        {
            // Is this the first run?
            if (_currentCharacterIndex == START_POS)
            {
                // Clear text box
                OysterMain.PlayerTalker!.SpeechDisplay.MainText.Text = string.Empty;
            }

            return base.Run();
        }
    }
}