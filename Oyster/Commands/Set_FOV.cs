using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_FOV : A_Command
    {
        // Private Variables
        private int _fov;

        // Constructor
        public Set_FOV(int fov)
        {
            // Pass Value
            _fov = fov;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length Check
            if (rawParameters.Length < 1) return null;

            // Cache
            int fov = default;

            // Read first value as int
            if (!LoadParameterValue(rawParameters[0], ref fov)) return null;

            // Make and return self
            return new Set_FOV(fov);
        }
        public override bool Run()
        {
            // Direct set FOV
            OysterMain.PlayerTalker!.Camera.FOV = _fov;

            // Return true
            return true;
        }
    }
}
