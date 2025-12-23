using Oyster.Core;
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
        /// <summary>
        /// Reads in a given set of raw parameters as though they were the parameters for a 'Set_Var' command.
        /// </summary>
        /// <returns>A valid string and value on success, null and default otherwise.</returns>
        public static (string? name, VariableType? value) ReadParams(string[] rawParameters)
        {
            // Length check
            if (rawParameters.Length < 2) return (null, default);

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
        public override bool Run()
        {
            // Set variable value
            Variables.SetVariableByName(_name, _value);

            // And finish
            return true;
        }
    }
}
