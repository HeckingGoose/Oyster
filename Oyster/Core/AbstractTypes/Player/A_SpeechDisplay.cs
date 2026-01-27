using Oyster.Core.Interfaces.Things;
using Oyster.Core.Types;

namespace Oyster.Core.AbstractTypes.Player
{
    public abstract class A_SpeechDisplay : IShowAndHide
    {
        // Const
        protected const string DEFAULT_DISPLAYNAME = "default";

        // Protected Variables
        protected ITextField _nameText;
        protected ITextField _mainText;
        protected IShowAndHide _nameTextBacking;
        protected IShowAndHide _mainTextBacking;
        protected IShowAndHide _continuePrompt;

        // Constructor
        public A_SpeechDisplay(
            ITextField nameText,
            ITextField mainText,
            IShowAndHide nameTextBacking,
            IShowAndHide mainTextBacking,
            IShowAndHide continuePrompt
            )
        {
            // Pass Values
            _nameText = nameText;
            _mainText = mainText;
            _nameTextBacking = nameTextBacking;
            _mainTextBacking = mainTextBacking;
            _continuePrompt = continuePrompt;

            // Hide on start
            Hide();
        }

        // Public Methods
        /// <summary>
        /// Resets this speech display to a mostly default state.
        /// </summary>
        /// <param name="nameText">The text to reset the 'name' text field to.</param>
        /// <param name="nameColour">The colour to reset the 'name' text field to.</param>
        public void ResetDisplay(string nameText, Colour nameColour)
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
        public void ResetDisplay(Colour nameColour) { ResetDisplay(DEFAULT_DISPLAYNAME, nameColour); }
        /// <summary>
        /// Resets this speech display to a mostly default state.
        /// </summary>
        public void ResetDisplay() { ResetDisplay(Colour.Red); }

        public void Show()
        {
            // Tell every part of the display to show themselves
            _nameText.Show();
            _nameTextBacking.Show();
            _mainText.Show();
            _mainTextBacking.Show();

            // Always hide prompt
            _continuePrompt.Hide();
        }
        public void Hide()
        {
            // Tell every part of the display to hide themselves
            _nameText.Hide();
            _nameTextBacking.Hide();
            _mainText.Hide();
            _mainTextBacking.Hide();

            // Always hide prompt
            _continuePrompt.Hide();
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
        /// <summary>
        /// Gets a reference to this display's 'continue' prompt.
        /// </summary>
        public IShowAndHide ContinuePrompt { get { return _continuePrompt; } }
    }
}
