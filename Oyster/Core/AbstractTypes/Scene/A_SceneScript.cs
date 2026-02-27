using Oyster.Core.AbstractTypes.Character;
using Oyster.Core.AbstractTypes.Player;
using Oyster.Core.Interfaces.Things;
using System;

namespace Oyster.Core.AbstractTypes.Scene
{
    public abstract class A_SceneScript
    {
        // Protected Variables
        protected IShowAndHide[] _thingsToBeHiddenMidConversation;
        protected A_Looker[] _lookers;

        // Constructor
        public A_SceneScript(
            IShowAndHide[] thingsToBeHiddenMidConversation,
            A_Looker[] lookers
            )
        {
            // Pass Values
            _thingsToBeHiddenMidConversation = thingsToBeHiddenMidConversation;
            _lookers = lookers;

            // Given the things are null, make a blank array
            if (_thingsToBeHiddenMidConversation == null) _thingsToBeHiddenMidConversation = Array.Empty<IShowAndHide>();
            if (_lookers == null) _lookers = Array.Empty<A_Looker>();
        }

        // Public Methods
        /// <summary>
        /// Starts a chat with the given characters.
        /// </summary>
        /// <param name="playerTalker">The player character that is being spoken to.</param>
        /// <param name="characterTalker">The character that will be doing the talking.</param>
        public void StartChat(
            A_PlayerTalker playerTalker,
            A_CharacterTalker characterTalker
            )
        { OysterMain.StartChat(this, playerTalker, characterTalker); }
        /// <summary>
        /// Hides all objects that should be hidden before starting conversation.
        /// </summary>
        public void HideObjectsForChat()
        {
            // Iterate every object that needs to be hidden, and then hide them
            foreach (IShowAndHide sah in _thingsToBeHiddenMidConversation) { sah.Hide(); }
        }
        /// <summary>
        /// Shows all objects that should be shown before starting conversation.
        /// </summary>
        public void ShowObjectsPostChat()
        {
            // Iterate every object that needs to be shown, and then show them
            foreach (IShowAndHide sah in _thingsToBeHiddenMidConversation) { sah.Show(); }
        }

        // Accessors
        /// <summary>
        /// Gets a list of lookers for the current scene.
        /// </summary>
        public A_Looker[] Lookers
        {
            get { return _lookers; }
        }
    }
}
