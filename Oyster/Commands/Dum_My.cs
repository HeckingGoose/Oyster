using Oyster.Core.Interfaces.Commands;

namespace Oyster.Commands
{
    internal class Dum_My : ISpeechCommand
    {
        // Public Methods
        public ISpeechCommand? CreateSelf(string[] rawParameters) { return MakeSelf(rawParameters); }
        public static ISpeechCommand? MakeSelf(string[] rawParameters) { return new Dum_My(); }
        public bool Run() { return true; }
    }
}
