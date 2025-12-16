using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_BoolVar : Set_Var<bool>
    {
        // Constructor
        public Set_BoolVar(string name, bool value) : base(name, value) { }

        // Public methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Use base class stuff
            (string? name, bool value) = ReadParams(rawParameters);

            // If name not null, then it's all good chief
            if (name == null) return null;

            // As it's all good just make it
            return new Set_BoolVar(name, value);
        }
    }
}
