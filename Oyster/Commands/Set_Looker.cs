using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_Looker : A_Command
    {
        // Private Variables
        private string _name;

        // Constructor
        public Set_Looker(string name)
        {
            // Pass Value
            _name = name;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length Check
            if (rawParameters.Length < 1) return null;

            // Cache
            string? name = string.Empty;

            // Read in first parameter as string
            if (!LoadParameterValue(rawParameters[0], ref name)) return null;

            // If name not null, then make and return self
            if (name == null) return null;
            return new Set_Looker(name);
        }
        public override bool Run()
        {
            // Direct set
            OysterMain.PlayerTalker!.Camera.LookTargetName = _name;

            // Now return true
            return true;
        }
    }
}
