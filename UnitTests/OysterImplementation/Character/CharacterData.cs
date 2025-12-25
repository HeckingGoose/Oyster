using Oyster.Core.AbstractTypes;
using Oyster.Core.AbstractTypes.Character;
using System.Drawing;

namespace UnitTests.OysterImplementation.Character
{
    internal class CharacterData : A_CharacterData
    {
        // Private Variables
        private string _script;

        // Constructor
        public CharacterData(
            string name,
            Color nameColour,
            float timeBetweenCharacters,
            string script
            ) : base(name, nameColour, timeBetweenCharacters)
        {
            // Pass Value
            _script = script;
        }

        // Public Methods
        public override A_BackgroundAssetLoader<string> BeginScriptLoad()
        {
            throw new NotImplementedException();
        }
        public override void SetScript(string scriptName)
        {
            throw new NotImplementedException();
        }
    }
}
