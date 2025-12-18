using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Meta : A_Command
    {
        // Const
        private const string PARAMETER_SCRIPTVERSION_NAME = "version";
        private const string PARAMETER_TARGETGAME_NAME = "game";

        // Private Variables
        private string _game;
        private string _version;

        // Constructor
        public Meta(string game, string version)
        {
            // Pass Values
            _game = game;
            _version = version;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Make dict
            Dictionary<string, (object value, Type type)> optionals = new Dictionary<string, (object value, Type type)>
            {
                { PARAMETER_TARGETGAME_NAME, (Definitions.SCRIPTVER_DEFAULT_GAME, typeof(string)) },
                { PARAMETER_SCRIPTVERSION_NAME, (Definitions.SCRIPTVER_DEFAULT_VERSION, typeof(string)) }
            };

            // Now read optionals
            LoadOptionalParameterValues(rawParameters, ref optionals);

            // And make self
            return new Meta(
                (string)optionals[PARAMETER_TARGETGAME_NAME].value,
                (string)optionals[PARAMETER_SCRIPTVERSION_NAME].value
                );
        }
        public override bool Run() { return true; }

        // Accessors
        public string Game { get { return _game; } }
        public string Version { get { return _version; } }
    }
}
