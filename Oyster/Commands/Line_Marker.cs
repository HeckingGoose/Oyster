using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Line_Marker : A_Command, ILineMarker
    {
        // Private Variables
        private string _name;

        // Constructor
        public Line_Marker(string name)
        {
            // Pass Value
            _name = name;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Read in first param as string
            string? name = null;
            bool success = LoadParameterValue(rawParameters[0], ref name);

            // Given failure return null
            if (!success || name == null) return null;

            // Otherwise return new self
            return new Line_Marker(name);
        }
        public override bool Run() { return true; }

        // Accessors
        public string Name { get { return _name; } }
    }
}
