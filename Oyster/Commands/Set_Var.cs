using Oyster.Core.AbstractTypes.Commands;

namespace Oyster.Commands
{
    public abstract class Set_Var<VariableType> : A_Command
    {
        // Private Variables
        protected string _name;
        protected VariableType _value;

        // Constructor
        protected Set_Var(
            string name,
            VariableType value
            )
        {
            // Pass values
            _name = name;
            _value = value;
        }

        // Public Methods
        public static (string? name, VariableType? value) ReadParams(string[] rawParameters)
        {
            // Declare stores
            string? name = string.Empty;
            VariableType? value = default;

            // Read in first value as string
            LoadParameterValue(rawParameters[0], ref name);

            // Check for fail
            if (name == null) return (null, default);

            // Read in second parameter as variable type (may not return null as could be int or bool soooo)
            bool success = LoadParameterValue(rawParameters[1], ref value);

            // Check fail
            if (!success) return (null, default);

            // Otherwise time to make
            return (name, value);
        }
    }
}
