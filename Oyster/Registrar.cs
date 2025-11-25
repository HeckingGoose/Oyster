using Oyster.Commands;
using Oyster.Core;

namespace Oyster
{
    internal static class Registrar
    {
        // Internal Methods
        /// <summary>
        /// Registers all built-in Oyster commands.
        /// </summary>
        internal static void RegisterCommands()
        {
            // Register all built-in commands
            OysterMain.AddCommand("act_speak", Act_Speak.MakeSelf);
            OysterMain.AddCommand("act_append", Act_Append.MakeSelf);
        }
    }
}
