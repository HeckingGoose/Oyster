namespace Oyster.Core
{
    internal static class Definitions
    {
        // Metadata Definitions
        internal const string VERSION_NUMBER_STRING = "4.1.0";
        internal const string VERSION_NAME_STRING = "Base";

        // OSF File Definitions
        internal const char OSF_VALID_LINEENDING = '\n';
        internal const string OSF_INVALID_LINEENDING = "\r";
        internal const int OSF_CLEANER_EXPECTEDSPLITSIZE = 2;
        internal const char OSF_PARAMETER_SEPERATOR = ',';
        internal const char OSF_COMMENT_CHARACTER = '#';
        internal const char OSF_COMMANDTODATA_SPLITTER = ' ';
        internal const char OSF_DATA_START = '[';
        internal const char OSF_DATA_END = ']';

        // Conversation Definitions
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

        // Rich Text Support
        internal const char RICHTEXT_TAGSTART = '<';
        internal const char RICHTEXT_TAGEND = '>';
        internal const char RICHTEXT_NEWLINE_PREFACE = '\\';
        internal const char RICHTEXT_NEWLINE_CHARACTER = 'n';

        // Script versioning
        internal const string SCRIPTVER_DEFAULT_GAME = "Unspecified";
        internal const string SCRIPTVER_DEFAULT_VERSION = "?.?.?";

        // Tickrate Definitions
        internal const int TICKS_PER_SECOND = 80;
        internal const float SECONDS_PER_TICK = 1f / TICKS_PER_SECOND;
    }
}
