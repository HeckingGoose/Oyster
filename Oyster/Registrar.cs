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
            OysterMain.AddCommand("set_intvar", Set_IntVar.MakeSelf);
            OysterMain.AddCommand("set_boolvar", Set_BoolVar.MakeSelf);
            OysterMain.AddCommand("set_stringvar", Set_StringVar.MakeSelf);
            OysterMain.AddCommand("line_marker", Line_Marker.MakeSelf);
            OysterMain.AddCommand("jump_to", Jump_To.MakeSelf);
            OysterMain.AddCommand("sys_wait", Sys_Wait.MakeSelf);
            OysterMain.AddCommand("meta", Meta.MakeSelf);
            OysterMain.AddCommand("set_colour", Set_Colour.MakeSelf);
            OysterMain.AddCommand("set_name", Set_Name.MakeSelf);
            OysterMain.AddCommand("set_script", Set_Script.MakeSelf);
            OysterMain.AddCommand("set_sprite", Set_Sprite.MakeSelf);
            OysterMain.AddCommand("set_looker", Set_Looker.MakeSelf);
        }
    }
}
