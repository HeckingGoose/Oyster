using Oyster.Core.Interfaces.Commands;

namespace Oyster.Core.AbstractTypes.Commands
{
    public abstract class A_Command : ISpeechCommand
    {


        // Constructor
        public A_Command() { }

        // Public methods
        public virtual ISpeechCommand MakeSelf(string[] rawParameters)
        {
            throw new NotImplementedException();
        }
        public abstract bool Run();
    }
}
