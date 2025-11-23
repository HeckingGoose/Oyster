using Oyster.Core.Interfaces.Commands;
using System.Diagnostics;

namespace Oyster.Core.AbstractTypes.Commands
{
    public abstract class A_Command : ISpeechCommand
    {
        // Constructor
        public A_Command() { }

        // Protected Methods
        protected string LoadStringContainingVariable(string rawParameterValue)
        {

        }
        protected VariableType? ReadParameter<VariableType>(string rawParameterValue) where VariableType : class
        {
            // Does the value start with a variable declaration
            if (rawParameterValue[0] == Definitions.PARAMETER_VARIABLE)
            {
                // Are we trying to load a string?
                if (typeof(VariableType) == typeof(string))
                {
                    // Then return it using this specific method
                    return (LoadStringContainingVariable(rawParameterValue) as VariableType)!;
                }

                // Otherwise let's just fetch the value
                (object? value, Type? type) = Variables.GetVariableByName(rawParameterValue.Substring(1));

                // Null?
                if (value == null || type == null)
                {
                    // Log issue
                    Debug.WriteLine($"Unable to load variable '{rawParameterValue.Substring(1)}', as variable does not exist!");
                    return null;
                }

                // Type mismatch?
                if (type != typeof(VariableType))
                {
                    // Log issue
                    Debug.WriteLine($"Unable to load variable '{rawParameterValue.Substring(1)}', as types do not match!");
                    return null;
                }

                // We can assume it's correct so return this
                return (VariableType)value;
            }

            // What type is being read in here?
            switch (typeof(VariableType))
            {
                // An integer
                case Type t when t == typeof(int):
                    // Parse it, on fail return null
                    if (int.TryParse(rawParameterValue, out int i)) return (VariableType)(object)i;
                    Debug.WriteLine($"Unable to parse parameter value '{rawParameterValue}' as an integer!");
                    return null;

                // A string
                case Type t when t == typeof(string):
                    // Are start and end character incorrect?
                    if (rawParameterValue[0] != Definitions.PARAMETER_STRING_DELIMINATOR &&
                        rawParameterValue[rawParameterValue.Length - 1] != Definitions.PARAMETER_STRING_DELIMINATOR)
                    {
                        // Return null
                        Debug.WriteLine($"Parameter value '{rawParameterValue}' is not a valid string!");
                        return null;
                    }

                    // Return trimmed string
                    return (VariableType)(object)rawParameterValue.Substring(1, rawParameterValue.Length - 2);

                // Boolean
                case Type t when t == typeof(bool):
                    // Parse it, on fail return null
                    if (bool.TryParse(rawParameterValue, out bool b)) return (VariableType)(object)b;
                    Debug.WriteLine($"Unable to parse parameter value '{rawParameterValue}' as a boolean!");
                    return null;
            }

            // Invalid type given
            Debug.WriteLine($"Invalid type '{nameof(VariableType)}' provided!");
            return null;
        }

        // Public Methods
        public abstract ISpeechCommand MakeSelf(string[] rawParameters);
        public abstract bool Run();
    }
}
