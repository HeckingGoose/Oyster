using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_Script : A_Command
    {
        // Private Variables
        private string _name;

        // Constructor
        public Set_Script(string name)
        {
            // Pass Value
            _name = name;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length check
            if (rawParameters.Length < 1) return null;

            // Cache
            string? name = string.Empty;

            // Read value as string, return null on fail
            if (!LoadParameterValue(rawParameters[0], ref name)) return null;

            // Now make self, unless name is somehow null
            if (name == null) return null;
            return new Set_Script(name);
        }
        public override bool Run()
        {
            // Direct set script name
            OysterMain.CharacterTalker!.Data.SetScript(_name);

            // Return true
            return true;
        }
    }
}
