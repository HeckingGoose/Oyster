using Oyster.Core;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_IntVar : Set_Var<int>
    {
        // Constructor
        public Set_IntVar(string name, int value) : base(name, value) { }

        // Explicit Interface Implementation
        public override ISpeechCommand? CreateSelf(string[] rawParameters)
        {
            return MakeSelf(rawParameters);
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParemeters)
        {
            // Use base class stuff
            (string? name, int value) = ReadParams(rawParemeters);

            // If name not null, then it's all good chief
            if (name == null) return null;

            // As it's all good just make it
            return new Set_IntVar(name, value);
        }
        public override bool Run()
        {
            // Set variable value
            Variables.SetVariableByName(_name, _value);

            // And finish
            return true;
        }
    }
}
