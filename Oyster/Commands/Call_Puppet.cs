using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Call_Puppet : A_Command
    {
        // Private Variables
        private string _command;

        // Constructor
        public Call_Puppet(string command)
        {
            // Pass Value
            _command = command;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length check
            if (rawParameters.Length < 1) return null;

            // Cache
            string? command = string.Empty;

            // Read in as string
            if (!LoadParameterValue(rawParameters[0], ref command)) return null;

            // If command not null, makek and return self
            if (command == null) return null;
            return new Call_Puppet(command);
        }
        public override bool Run()
        {
            // Raise puppet call with command name
            OysterMain.CallPuppet(_command);

            // Return true
            return true;
        }
    }
}
