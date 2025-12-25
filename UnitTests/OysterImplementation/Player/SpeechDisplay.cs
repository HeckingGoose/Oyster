using Oyster.Core.AbstractTypes.Player;
using Oyster.Core.Interfaces.Things;

namespace UnitTests.OysterImplementation.Player
{
    internal class SpeechDisplay : A_SpeechDisplay
    {
        // Constructor
        public SpeechDisplay(
            ITextField nameText,
            ITextField mainText,
            IShowAndHide nameTextBacking,
            IShowAndHide mainTextBacking,
            IShowAndHide continuePrompt
            ) : base(nameText, mainText, nameTextBacking, mainTextBacking, continuePrompt)
        {

        }
    }
}
