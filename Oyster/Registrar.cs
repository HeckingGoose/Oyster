using Oyster.Commands;
using Oyster.Core;

namespace Oyster
{
    internal static class Registrar
    {
        // Constructor
        static Registrar()
        {
            // Has Oyster finished on the bog?
            if (OysterMain.Loaded)
            {
                RegisterCommands();
            }

            // If not then wait for it
            else
            {
                OysterMain.OnLoaded += RegisterCommands;
            }
        }

        // Private Methods
        /// <summary>
        /// Registers all built-in Oyster commands.
        /// </summary>
        private static void RegisterCommands()
        {
            // Register all built-in commands
            OysterMain.AddCommand("act_speak", Act_Speak.MakeSelf);
            OysterMain.AddCommand("act_append", Act_Append.MakeSelf);
        }
    }
}
