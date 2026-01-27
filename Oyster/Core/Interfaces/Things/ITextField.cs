using Oyster.Core.Types;

namespace Oyster.Core.Interfaces.Things
{
    public interface ITextField : IShowAndHide
    {
        // Public Methods
        /// <summary>
        /// Clears this text field to be empty.
        /// </summary>
        public void Clear();

        // Accessors
        /// <summary>
        /// Gets or sets the text for this text field.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets the colour of this text field.
        /// </summary>
        public Colour TextColour { get; set; }
    }
}
