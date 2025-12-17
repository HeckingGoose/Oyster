using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Jump_To : A_Command
    {
        // Private Variables
        private string _name;

        // Constructor
        public Jump_To(string name)
        {
            // Pass Value
            _name = name;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Read first parameter as string
            string? name = null;
            bool success = LoadParameterValue(rawParameters[0], ref name);

            // On fail return null
            if (!success || name == null) return null;

            // Make self and return
            return new Jump_To(name);
        }
        public override bool Run()
        {
            // Attempt jump
            OysterMain.JumpToLineMarker(_name);

            // Now return true
            return true;
        }
    }
}
