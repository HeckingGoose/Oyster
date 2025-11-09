using Oyster.ImplementationInterfaces;
using System.Drawing;

namespace Oyster.AbstractTypes
{
    public class A_SpeechDisplay : IShowAndHide
    {
        // Const
        protected const string DEFAULT_DISPLAYNAME = "default";

        // Protected Variables
        protected ITextField _nameText;
        protected ITextField _mainText;
        protected IShowAndHide _nameTextBacking;
        protected IShowAndHide _mainTextBacking;

        // Constructor
        public A_SpeechDisplay(
            ITextField nameText,
            ITextField mainText,
            IShowAndHide nameTextBacking,
            IShowAndHide mainTextBacking
            )
        {
            // Pass Values
            _nameText = nameText;
            _mainText = mainText;
            _nameTextBacking = nameTextBacking;
            _mainTextBacking = mainTextBacking;
        }

        // Public Methods
        /// <summary>
        /// Resets this speech display to a mostly default state.
        /// </summary>
        /// <param name="nameText">The text to reset the 'name' text field to.</param>
        /// <param name="nameColour">The colour to reset the 'name' text field to.</param>
        public void ResetDisplay(string nameText, Color nameColour)
        {
            // Pass Values
            _nameText.Text = nameText;
            _nameText.TextColour = nameColour;

            // Now set defaults
            _mainText.Clear();
        }
        /// <summary>
        /// Resets this speech display to a mostly default state.
        /// </summary>
        /// <param name="nameColour">The colour to reset the 'name' text field to.</param>
        public void ResetDisplay(Color nameColour) { ResetDisplay(DEFAULT_DISPLAYNAME, nameColour); }
        /// <summary>
        /// Resets this speech display to a mostly default state.
        /// </summary>
        public void ResetDisplay() { ResetDisplay(Color.Red); }

        public void Show()
        {
            // Tell every part of the display to show themselves
            _nameText.Show();
            _nameTextBacking.Show();
            _mainText.Show();
            _mainTextBacking.Show();
        }
        public void Hide()
        {
            // Tell every part of the display to hide themselves
            _nameText.Hide();
            _nameTextBacking.Hide();
            _mainText.Hide();
            _mainTextBacking.Hide();
        }

        // Accessors
        /// <summary>
        /// Gets a reference to this display's 'name' text field.
        /// </summary>
        public ITextField NameText { get { return _nameText; } }
        /// <summary>
        /// Gets a reference to this display's 'main' text field.
        /// </summary>
        public ITextField MainText { get { return _mainText; } }
    }
}
