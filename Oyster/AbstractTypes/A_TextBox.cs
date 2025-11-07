using Oyster.ImplementationInterfaces;

namespace Oyster.AbstractTypes
{
    public class A_TextBox
    {
        // Protected Variables
        protected ITextField _nameText;
        protected ITextField _mainText;

        // Constructor
        public A_TextBox(ITextField nameText, ITextField mainText)
        {
            // Pass Values
            _nameText = nameText;
            _mainText = mainText;

        }
    }
}
