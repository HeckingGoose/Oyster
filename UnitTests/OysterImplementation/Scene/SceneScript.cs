using Oyster.Core.AbstractTypes.Scene;
using Oyster.Core.Interfaces.Things;

namespace UnitTests.OysterImplementation.Scene
{
    public class SceneScript : A_SceneScript
    {
        // Constructor
        public SceneScript(
            IShowAndHide[] thingsToBeHiddenMidConversation
            ) : base(thingsToBeHiddenMidConversation)
        {

        }
    }
}
