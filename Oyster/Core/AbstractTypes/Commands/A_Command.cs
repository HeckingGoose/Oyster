using Oyster.Core.Interfaces.Commands;
using System.Diagnostics;

namespace Oyster.Core.AbstractTypes.Commands
{
    public abstract class A_Command : ISpeechCommand
    {
        // Constructor
        public A_Command() { }

        // Protected Methods
        /// <summary>
        /// Loads a parameter into a given variable of matching type.
        /// </summary>
        protected void LoadParameterValue<VariableType>(string rawValue, ref VariableType? destination) where VariableType : class
        {
            // Load parameter
            destination = ReadParameter<VariableType>(rawValue);
        }
        /// <summary>
        /// Parses a string for any variable declarations and inlines their value within the string.
        /// </summary>
        /// <param name="rawParameterValue">A string representing the parameter value.</param>
        /// <returns>A valid string containing at least the base string content, along with any successfully read variables inlined.</returns>
        protected string LoadStringContainingVariables(string rawParameterValue)
        {
            // Declare output
            string output = string.Empty;

            // Iterate every character in the string
            bool readingVariable = false;
            string variableName = string.Empty;
            foreach (char c in rawParameterValue)
            {
                // What state should we be in?
                switch (readingVariable)
                {
                    // We are reading a variable
                    case true:
                        // Is this character the end of a variable
                        if (c == Definitions.PARAMETER_VARIABLE_INLINE_END)
                        {
                            // Then commit variable and move to reading regular text
                            readingVariable = false;

                            // Get variable value
                            (object? value, Type? t) = Variables.GetVariableByName(variableName);

                            // If it's null then we should just dump a default value
                            if (value == null || t == null)
                            {
                                output += Definitions.VARIABLE_NOTEXISTS_FORSTRING;
                                break;
                            }

                            // Since it's not null we should convert it to a string and add
                            output += value.ToString();
                            break;
                        }

                        // Add character to variable name
                        variableName += c;
                        break;

                    // We are reading just a regular part of the string
                    case false:
                        // Is this character the start of a variable?
                        if (c == Definitions.PARAMETER_VARIABLE_INLINE_START)
                        {
                            // Then move to reading variables
                            readingVariable = true;
                            variableName = string.Empty;
                            break;
                        }

                        // Copy value
                        output += c;
                        break;
                }
            }

            // We should now have a properly formatted string
            return output;
        }
        /// <summary>
        /// Reads a raw parameter in as the given type.
        /// </summary>
        /// <typeparam name="VariableType">The type to load the parameter as.</typeparam>
        /// <param name="rawParameterValue">A string representing the parameter value.</param>
        /// <returns>A valid value on success, null on any failure.</returns>
        protected VariableType? ReadParameter<VariableType>(string rawParameterValue) where VariableType : class
        {
            // Does the value start with a variable declaration
            if (rawParameterValue[0] == Definitions.PARAMETER_VARIABLE)
            {
                // Are we trying to load a string containing variables
                if (typeof(VariableType) == typeof(string) && rawParameterValue[1] == '"')
                {
                    // Then return it using this specific method
                    return (LoadStringContainingVariables(rawParameterValue.Substring(2, rawParameterValue.Length - 3)) as VariableType)!;
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
