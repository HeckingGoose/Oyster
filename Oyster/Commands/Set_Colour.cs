using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;
using System.Drawing;

namespace Oyster.Commands
{
    public class Set_Colour : A_Command
    {
        // Private Variables
        private Color _colour;

        // Constructor
        public Set_Colour(
            int r,
            int g,
            int b,
            int a
            )
        {
            // Make Colour
            _colour = Color.FromArgb(a, r, g, b);
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length check
            if (rawParameters.Length < 4) return null;

            // Create stores
            int r = default;
            int g = default;
            int b = default;
            int a = default;

            // Read in rgba values, return null on fail
            if (!LoadParameterValue(rawParameters[0], ref r)) return null;
            if (!LoadParameterValue(rawParameters[1], ref g)) return null;
            if (!LoadParameterValue(rawParameters[2], ref b)) return null;
            if (!LoadParameterValue(rawParameters[3], ref a)) return null;

            // Now make command
            return new Set_Colour(r, g, b, a);
        }
        public override bool Run()
        {
            // Direct set main colour
            OysterMain.PlayerTalker!.SpeechDisplay.NameText.TextColour = _colour;

            // And return true
            return true;
        }
    }
}
