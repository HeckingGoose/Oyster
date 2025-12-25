using Oyster.Core.Interfaces.Things;
using System.Drawing;

namespace UnitTests.OysterImplementation.Object
{
    internal class TextField : ITextField
    {
        // Const
        private string DEFAULT_TEXT = string.Empty;
        private Color DEFAULT_COLOUR = Color.Black;

        // Private Variables
        private string _text;
        private Color _colour;

        // Constructor
        public TextField()
        {
            // Pass Defaults
            _text = DEFAULT_TEXT;
            _colour = DEFAULT_COLOUR;
        }

        // Public Methods
        public void Clear()
        {
            // Set text to string.empty
            _text = string.Empty;
        }
        public void Hide()
        {
            // Do nothing
        }
        public void Show()
        {
            // Do nothing
        }

        // Accessors
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public Color TextColour
        {
            get { return _colour; }
            set { _colour = value; }
        }
    }
}
