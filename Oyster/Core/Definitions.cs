namespace Oyster.Core
{
    internal class Definitions
    {
        // Conversation Definitions
        internal const char LINE_ENDING = '\n';
        internal const char COMMAND_DATA_SPLITTER = ' ';
        internal const char DATA_START_CHAR = '[';
        internal const char DATA_END_CHAR = ']';
        internal const char COMMENT_CHAR = '#';
        internal const string BAD_LINE_ENDING = "\r";
        internal const int EXPECTED_SPLIT_SIZE = 2;
        internal const int PROMPT_WAIT_TIME = 4;

        // Parameter Formatting Definitions
        internal const char PARAMETER_SEPERATOR = ',';
        internal const char PARAMETER_VARIABLE = '$';
        internal const char PARAMETER_STRING_DELIMINATOR = '\"';
        internal const char PARAMETER_VARIABLE_INLINE_START = '{';
        internal const char PARAMETER_VARIABLE_INLINE_END = '}';
        internal const char PARAMETER_NAMEVALUE_SPLITTER = '=';

        // Variable stuff
        internal const string VARIABLE_NOTEXISTS_FORSTRING = "???";
    }
}
