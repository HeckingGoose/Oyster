using Oyster.Core.Interfaces.Commands;
using System.Diagnostics;

namespace Oyster.Core.AbstractTypes.Commands
{
    public abstract class A_Command : ISpeechCommand
    {
        // Protected Variables
        protected Dictionary<string, (object value, Type type)> _optionalParameters;

        // Constructor
        public A_Command() { _optionalParameters = new Dictionary<string, (object value, Type type)>(); }

        // Protected Methods
        /// <summary>
        /// Loads a parameter into a given variable of matching type.
        /// </summary>
        protected static bool LoadParameterValue<VariableType>(string rawValue, ref VariableType? destination)
        {
            // Load parameter
            (destination, bool success) = ReadParameter<VariableType>(rawValue);
            return success;
        }
        /// <summary>
        /// Given a dictionary of parameters and optional parameter inputs, matches the inputs with dictionary entries and updates the dictionary to these new inputs.
        /// </summary>
        protected static void LoadOptionalParameterValues(string[] optionalParameters, ref Dictionary<string, (object value, Type type)> destination)
        {
            // Iterate every parameter
            for (int i = 1; i < optionalParameters.Length; i++)
            {
                // Split across the splitter
                string[] split = SplitIntoVariableAndData(optionalParameters[i]);

                // Iterate every value in the dictionary
                foreach (KeyValuePair<string, (object value, Type type)> kvp in destination)
                {
                    // Check if split is right size. If not then skip it.
                    if (split.Length != 2) continue;

                    // Check if the key matches
                    if (split[0] == kvp.Key)
                    {
                        // What type is this?
                        switch (kvp.Value.type)
                        {
                            // Boolean
                            case Type t when t == typeof(bool):
                                destination[kvp.Key] = (ReadParameter<bool>(split[1]), kvp.Value.type);
                                break;

                            // Int
                            case Type t when t == typeof(int):
                                destination[kvp.Key] = (ReadParameter<int>(split[1]), kvp.Value.type);
                                break;

                            // String
                            case Type t when t == typeof(string):
                                // Cache value
                                object o = kvp.Value.value!;

                                // Do thing
                                destination[kvp.Key] = (ReadParameter<string>(split[1]), kvp.Value.type)!;

                                // If null then use default
                                if (destination[kvp.Key].value == null) destination[kvp.Key] = (o, kvp.Value.type);
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Given a string, parses it for possible RTTs.
        /// </summary>
        /// <param name="line">The line to parse.</param>
        /// <param name="startPoint">The character to start on.</param>
        /// <returns>The character at index startPoint if the character there does not suggest RTT.
        /// A RTT tag if the character does suggest an RTT tag.
        /// Also supports \n.</returns>
        protected static string ParseForRTT(string line, int startPoint)
        {
            // Create output store (we will always return at least the first character
            string output = line.Substring(startPoint, 1);

            // Is this a RTT?
            if (line[startPoint] == Definitions.RICHTEXT_TAGSTART)
            {
                // Then we need to add characters until we find the end of the tag
                for (int i = startPoint + 1; i < line.Length; i++)
                {
                    // Add to string
                    output += line[i];

                    // Is this the end tag?
                    if (line[i] == Definitions.RICHTEXT_TAGEND)
                    {
                        // It's time to leave
                        break;
                    }
                }
            }

            // Is this a \n?
            else if (line[startPoint] == Definitions.RICHTEXT_NEWLINE_PREFACE &&
                line.Length > startPoint + 1 && // Length check in case it's just a line that ends with '\'
                line[startPoint + 1] == Definitions.RICHTEXT_NEWLINE_CHARACTER)
            {
                // Add the next character then as well
                output += line[startPoint + 1];
            }

            // Return output
            return output;
        }

        // Private Methods
        /// <summary>
        /// Splits the given string into a name and a parameter value.
        /// </summary>
        private static string[] SplitIntoVariableAndData(string raw)
        {
            return raw.Split(Definitions.PARAMETER_NAMEVALUE_SPLITTER, 2);
        }
        /// <summary>
        /// Parses a string for any variable declarations and inlines their value within the string.
        /// </summary>
        /// <param name="rawParameterValue">A string representing the parameter value.</param>
        /// <returns>A valid string containing at least the base string content, along with any successfully read variables inlined.</returns>
        private static string LoadStringContainingVariables(string rawParameterValue)
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
        /// <returns>A valid value on success, default type value on any failure.</returns>
        private static (VariableType? variable, bool success) ReadParameter<VariableType>(string rawParameterValue)
        {
            // Does the value start with a variable declaration
            if (rawParameterValue[0] == Definitions.PARAMETER_VARIABLE)
            {
                // Are we trying to load a string containing variables
                if (typeof(VariableType) == typeof(string) && rawParameterValue[1] == '"')
                {
                    // Then return it using this specific method
                    return (((VariableType)(object)LoadStringContainingVariables(rawParameterValue.Substring(2, rawParameterValue.Length - 3)))!, true);
                }

                // Otherwise let's just fetch the value
                (object? value, Type? type) = Variables.GetVariableByName(rawParameterValue.Substring(1));

                // Null?
                if (value == null || type == null)
                {
                    // Log issue
                    Debug.WriteLine($"Unable to load variable '{rawParameterValue.Substring(1)}', as variable does not exist!");
                    return (default, false);
                }

                // Type mismatch?
                if (type != typeof(VariableType))
                {
                    // Log issue
                    Debug.WriteLine($"Unable to load variable '{rawParameterValue.Substring(1)}', as types do not match!");
                    return (default, false);
                }

                // We can assume it's correct so return this
                return ((VariableType)value, true);
            }

            // What type is being read in here?
            switch (typeof(VariableType))
            {
                // An integer
                case Type t when t == typeof(int):
                    // Parse it, on fail return null
                    if (int.TryParse(rawParameterValue, out int i)) return ((VariableType)(object)i, true);
                    Debug.WriteLine($"Unable to parse parameter value '{rawParameterValue}' as an integer!");
                    return (default, false);

                // A string
                case Type t when t == typeof(string):
                    // Are start and end character incorrect?
                    if (rawParameterValue[0] != Definitions.PARAMETER_STRING_DELIMINATOR &&
                        rawParameterValue[rawParameterValue.Length - 1] != Definitions.PARAMETER_STRING_DELIMINATOR)
                    {
                        // Return null
                        Debug.WriteLine($"Parameter value '{rawParameterValue}' is not a valid string!");
                        return (default, false);
                    }

                    // Return trimmed string
                    return ((VariableType)(object)rawParameterValue.Substring(1, rawParameterValue.Length - 2), true);

                // Boolean
                case Type t when t == typeof(bool):
                    // Parse it, on fail return null
                    if (bool.TryParse(rawParameterValue, out bool b)) return ((VariableType)(object)b, true);
                    Debug.WriteLine($"Unable to parse parameter value '{rawParameterValue}' as a boolean!");
                    return (default, false);
            }

            // Invalid type given
            Debug.WriteLine($"Invalid type '{nameof(VariableType)}' provided!");
            return default;
        }

        // Public Methods
        //public abstract ISpeechCommand? CreateSelf(string[] rawParameters);
        public abstract bool Run();
    }
}
