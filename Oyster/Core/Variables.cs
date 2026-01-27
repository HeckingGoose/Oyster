using System;
using System.Collections.Generic;

namespace Oyster.Core
{
    public static class Variables
    {
        // Private Variables
        private static Dictionary<string, (object value, Type type)> _variables;

        // Constructor
        static Variables() { _variables = new Dictionary<string, (object, Type)>(); }

        // Public Methods
        /// <summary>
        /// Fetches the value of a named variable.
        /// </summary>
        /// <param name="name">The name of the variable to fetch.</param>
        /// <returns>An object containing the variable value and the type of the variable on success, two null values on failure.</returns>
        public static (object? value, Type? type) GetVariableByName(string name)
        {
            // Does the key exist? If so then return value
            if (_variables.ContainsKey(name)) return _variables[name];

            // If not then return null
            else return (null, null);
        }
        /// <summary>
        /// Creates a named variable with a given value.
        /// </summary>
        public static void SetVariableByName<VariableType>(string name, VariableType value)
        {
            // Does dictionary contain this key? If so update value
            if (_variables.ContainsKey(name)) _variables[name] = (value, typeof(VariableType))!;

            // If it does not then add it
            else _variables.Add(name, (value, typeof(VariableType))!);
        }
    }
}
