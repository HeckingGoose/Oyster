using Oyster.Core;
using Oyster.Core.AbstractTypes.Commands;
using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    public class Set_Sprite : A_Command
    {
        // Private Variables
        private string _name;

        // Constructor
        public Set_Sprite(string name)
        {
            // Pass Value
            _name = name;
        }

        // Public Methods
        public static ISpeechCommand? MakeSelf(string[] rawParameters)
        {
            // Length check
            if (rawParameters.Length < 1) return null;

            // Cache
            string? name = string.Empty;

            // Read first param as string
            if (!LoadParameterValue(rawParameters[0], ref name)) return null;

            // Make and return self if name not null
            if (name == null) return null;
            return new Set_Sprite(name);
        }
        public override bool Run()
        {
            // Direct set sprite via name
            OysterMain.CharacterTalker!.SpriteManager.SetSprite(_name);

            // Return true
            return true;
        }
    }
}
