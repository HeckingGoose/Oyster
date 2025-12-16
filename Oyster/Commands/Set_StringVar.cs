using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_StringVar : Set_Var<string>
    {
        // Constructor
        public Set_StringVar(string name, string value) : base(name, value) { }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Use base class stuff
            (string? name, string? value) = ReadParams(rawParameters);

            // If name and value not null, then it's all good chief
            if (name == null || value == null) return null;

            // As it's all good just make it
            return new Set_StringVar(name, value);
        }
    }
}
