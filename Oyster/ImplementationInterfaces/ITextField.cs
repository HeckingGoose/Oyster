using System.Drawing;

namespace Oyster.ImplementationInterfaces
{
    public interface ITextField
    {
        // Public Methods
        public void Clear();

        // Accessors
        public string Text { get; set; }
        public Color TextColour { get; set; }
    }
}
