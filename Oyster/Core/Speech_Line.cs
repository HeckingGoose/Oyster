using System.Collections.Generic;

namespace Oyster.Core
{
    public class Speech_Line
    {
        // Private Variables
        private string _commandName;
        private string[] _parameters;

        // Constructor
        public Speech_Line(
            string function,
            string rawParameters
            )
        {
            // Pass in values
            _commandName = function;
            _parameters = ReadInParameters(rawParameters);
        }

        // Private Methods
        private static string[] ReadInParameters(string rawParameters)
        {
            // Let's make a temp store
            string temp = string.Empty;
            bool listenToCommas = true;
            List<string> parameters = new List<string>();

            // Loop through every character in the parameters
            foreach (char c in rawParameters)
            {
                // Is this a string deliminator?
                if (c == Definitions.PARAMETER_STRING_DELIMINATOR)
                {
                    // Then switch whether we should listen to commas
                    listenToCommas = !listenToCommas;

                    // Add the character
                    temp += c;
                }

                // Is this a comma?
                else if (c == Definitions.OSF_PARAMETER_SEPERATOR && listenToCommas)
                {
                    // Well then dump our string
                    parameters.Add(temp);
                    temp = string.Empty;
                }

                // Otherwise
                else
                {
                    // This is a whitespace and we are not reading a string
                    if (c == ' ' && listenToCommas)
                    {
                        // Ignore it
                        continue;
                    }

                    // Otherwise let's read it
                    temp += c;
                }
            }

            // Dump what we have left, if we have anything left
            if (temp.Length > 0)
            {
                parameters.Add(temp);
            }

            // And return
            return parameters.ToArray();
        }

        // Accessors
        /// <summary>
        /// Returns the intended function of this line.
        /// </summary>
        public string CommandName
        {
            get
            {
                return _commandName;
            }
        }
        /// <summary>
        /// Returns the parameters for this line.
        /// </summary>
        public string[] Parameters
        {
            get
            {
                return _parameters;
            }
        }
    }
}