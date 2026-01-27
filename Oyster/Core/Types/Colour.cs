namespace Oyster.Core.Types
{
    public class Colour
    {
        // Static
        private readonly static Colour S_Red = new Colour(255, 0, 0, 255);

        // Private Variables
        private byte _red;
        private byte _green;
        private byte _blue;
        private byte _alpha;

        // Constructor
        public Colour(byte red, byte green, byte blue, byte alpha)
        {
            // Pass Values
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = alpha;
        }

        // Accessors
        /// <summary>
        /// Gets or sets the value of the red channel for this colour.
        /// </summary>
        public byte ByteRed { get { return _red; } set { _red = value; } }
        /// <summary>
        /// Gets or sets the value of the green channel for this colour.
        /// </summary>
        public byte ByteGreen { get { return _green; } set { _green = value; } }
        /// <summary>
        /// Gets or sets the value of the blue channel for this colour.
        /// </summary>
        public byte ByteBlue { get { return _blue; } set { _blue = value; } }
        /// <summary>
        /// Gets or sets the value of the alpha channel for this colour.
        /// </summary>
        public byte ByteAlpha { get { return _alpha; } set { _alpha = value; } }

        // Accessors
        public static Colour Red { get { return S_Red; } }
    }
}
