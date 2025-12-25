using Oyster.Core.AbstractTypes.Player;
using UnitTests.OysterImplementation.Object;

namespace UnitTests.OysterImplementation.Player
{
    internal class PlayerTalker : A_PlayerTalker
    {
        // Constructor
        public PlayerTalker(
            Camera camera,
            SpeechDisplay speechDisplay
            ) : base(camera, speechDisplay)
        {

        }
    }
}
