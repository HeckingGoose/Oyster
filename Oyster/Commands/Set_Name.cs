using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_Name : A_Command
    {
        // Private Variables
        private string _name;

        // Constructor
        public Set_Name(string name)
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

            // Read value as string, on fail return null
            if (!LoadParameterValue(rawParameters[0], ref name)) return null;

            // Make command, given name not null
            if (name == null) return null;
            return new Set_Name(name);
        }
        public override bool Run()
        {
            // Direct set name
            OysterMain.PlayerTalker!.SpeechDisplay.NameText.Text = _name;

            // Return true
            return true;
        }
    }
}
