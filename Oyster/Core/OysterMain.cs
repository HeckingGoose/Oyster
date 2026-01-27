using Oyster.Commands;
using Oyster.Core.AbstractTypes;
using Oyster.Core.AbstractTypes.Character;
using Oyster.Core.AbstractTypes.Player;
using Oyster.Core.AbstractTypes.Scene;
using Oyster.Core.Interfaces.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Oyster.Core
{
    public static class OysterMain
    {
        // Enum
        public enum SpeechState
        {
            NotTalking,
            Loading,
            Talking
        }

        // Delegates
        public delegate ISpeechCommand? CommandCreationDelegate(string[] parameters);
        public delegate void PuppetCall(string command);
        public delegate void BlankDelegate();
        public delegate (string name, string number) VersionGetDelegate();

        // Events
        public static event PuppetCall? OnPuppetCalled;
        public static event BlankDelegate? OnSpeechCompleted;
        public static event BlankDelegate? OnScriptReady;
        public static event VersionGetDelegate OnVersionGetRequest;
        public static event BlankDelegate? OnLoaded;

        // Private Variables
        // < General >
        private static bool _loaded = false;
        private static SpeechState _oysterState;
        private static float _timeSinceLastFrame;
        // < Scene Objects >
        private static A_SceneScript? _sceneScript;
        private static A_PlayerTalker? _playerScript;
        private static A_CharacterTalker? _characterScript;
        // < Conversation Loading >
        private static A_BackgroundAssetLoader<string>? _scriptLoader;
        private static Speech_Line[]? _rawScript;
        private static Dictionary<string, CommandCreationDelegate> _validCommands;
        // < Conversation Objects >
        private static ISpeechCommand?[] _script;
        private static string _scriptGame = Definitions.SCRIPTVER_DEFAULT_GAME;
        private static string _scriptVersion = Definitions.SCRIPTVER_DEFAULT_VERSION;
        private static Dictionary<string, int>? _lineMarkers;
        private static int _currentCommandIndex;
        private static int _nextCommandToLoadIndex;
        private static bool _safeToLoadMoreCommands;
        private static int _promptWaitTimer;

        // Constructor
        static OysterMain()
        {
            // Set event
            OnVersionGetRequest = InternalNumberAndName;

            // Set start state
            _oysterState = SpeechState.NotTalking;

            // Default array values
            _script = Array.Empty<ISpeechCommand>();
            _validCommands = new Dictionary<string, CommandCreationDelegate>();

            // Call events for when Oyster is loaded
            _loaded = true;
            if (OnLoaded != null) OnLoaded();

            // Tell registrar to register commands
            Registrar.RegisterCommands();
        }

        // Private Methods
        /// <summary>
        /// Generates a dictionary containing all line markers within a given set of Speech_Lines. Is relatively expensive as it generates the entire script's commands to do this. Call sparingly.
        /// </summary>
        private static Dictionary<string, int> GenLineMarkers(Speech_Line[] lines)
        {
            // Make store
            Dictionary<string, int> lineMarkers = new Dictionary<string, int>();

            // Go through entire length of script
            for (int i = 0; i < lines.Length; i++)
            {
                // Make command
                ISpeechCommand c = LoadCommand(i, _rawScript!);

                // Now check if this is a line marker, if it is we need to cache it
                if (c is ILineMarker) lineMarkers.Add((c as ILineMarker)!.Name, i);
            }

            // And return
            return lineMarkers;
        }
        /// <summary>
        /// Generates two strings representing the target game and version of a given oscript. Is relatively expensive as it has to generate the entire script's commands to do this. Call sparingly.
        /// </summary>
        private static (string game, string version) GenScriptVersion(Speech_Line[] lines)
        {
            // Make stores
            string game = Definitions.SCRIPTVER_DEFAULT_GAME;
            string version = Definitions.SCRIPTVER_DEFAULT_VERSION;

            // Go through entire length of script
            for (int i = 0; i < lines.Length; i++)
            {
                // Make command
                ISpeechCommand c = LoadCommand(i, _rawScript!);

                // Now check if this is a meta tag
                if (c is Meta)
                {
                    // Cache
                    Meta m = (Meta)c;

                    // Then pass value if different
                    game = m.Game != Definitions.SCRIPTVER_DEFAULT_GAME ? m.Game : game;
                    version = m.Version != Definitions.SCRIPTVER_DEFAULT_VERSION ? m.Version : version;
                }
            }

            // And return
            return (game, version);
        }
        /// <summary>
        /// Translates raw input into a slightly neater format. Ensures that all lines are somewhat valid commands.
        /// </summary>
        private static Speech_Line[] RawToLines(string[] rawLines)
        {
            // Create store (not matching input size due to some lines maybe needing to be ignored).
            List<Speech_Line> output = new List<Speech_Line>();

            // Iterate through every line
            foreach (string line in rawLines)
            {
                // Does the line have absolutely anything on it??
                if (line == null || line == string.Empty)
                {
                    // If not then skip it
                    continue;
                }

                // Does this line begin with a comment?
                if (line[0] == Definitions.OSF_COMMENT_CHARACTER)
                {
                    // Skip it
                    continue;
                }

                // Check for bad lines in general
                if (line == Definitions.OSF_INVALID_LINEENDING) { continue; }

                // Remove trailing \r
                string clean = line.Split(Definitions.OSF_INVALID_LINEENDING, Definitions.OSF_CLEANER_EXPECTEDSPLITSIZE)[0];

                // Attempt to split across the first space
                string[] split = clean.Split(Definitions.OSF_COMMANDTODATA_SPLITTER, Definitions.OSF_CLEANER_EXPECTEDSPLITSIZE);

                // Length check
                if (split.Length < Definitions.OSF_CLEANER_EXPECTEDSPLITSIZE)
                {
                    // Skip
                    continue;
                }

                // Check that the first and last characters match QED
                if (split[1][0] != Definitions.OSF_DATA_START ||
                    split[1][split[1].Length - 1] != Definitions.OSF_DATA_END)
                {
                    // Given that they don't then discard this line
                    continue;
                }

                // Remove those characters
                split[1] = split[1].Substring(1, split[1].Length - 2);

                // Attempt to fetch the command
                string command = string.Empty;
                split[0] = split[0].ToLower();
                if (_validCommands.ContainsKey(split[0])) command = split[0];

                // Is this command valid?
                if (command == string.Empty)
                {
                    // If not then skip this line
                    Debug.WriteLine($"Unknown command '{split[0]}' found during linting!");
                    continue;
                }

                // Make a new line from these
                output.Add(
                    new Speech_Line(
                        command,
                        split[1]
                        )
                    );
            }
            return output.ToArray();
        }
        /// <summary>
        /// Loads a command from 'rawScript' at the given 'index'.
        /// </summary>
        private static ISpeechCommand LoadCommand(int index, Speech_Line[] rawScript)
        {
            // Is the next command out of range? If it is then just skip.
            if (index >= rawScript.Length || index < 0) { return new Dum_My(); }

            // Otherwise let's figure out what this command is
            // As we know it will be a valid command we can directly index and create it
            ISpeechCommand? command = _validCommands[rawScript[index].CommandName].Invoke(rawScript[index].Parameters);

            // If command fails to create then use a dummy command that takes one tick and does nothing
            if (command == null) command = new Dum_My();

            // Finally, pass command
            return command;
        }
        /// <summary>
        /// Loads the next command.
        /// </summary>
        private static void LoadNextCommand()
        {
            // Is the next command out of range? If it is then just skip. (And mark it as unsafe to read more).
            if (_nextCommandToLoadIndex >= _script.Length) { _safeToLoadMoreCommands = false; return; }

            // Load command
            ISpeechCommand? command = LoadCommand(_nextCommandToLoadIndex, _rawScript!);

            // Finally, pass command
            _script[_nextCommandToLoadIndex] = command;

            // Is this command something that modifies variables? If so then we can't load more commands, as they may depend on variables existing.
            if (_script[_nextCommandToLoadIndex] is IModifiesVariables) _safeToLoadMoreCommands = false;

            // And increment to the next line
            _nextCommandToLoadIndex++;
        }
        /// <summary>
        /// Called when _scriptLoader finishes.
        /// </summary>
        private static void OnScriptLoaded(A_BackgroundAssetLoader<string>.LoadResult loadResult, string log)
        {
            // Did it load successfully?
            switch (loadResult)
            {
                // Yes
                case A_BackgroundAssetLoader<string>.LoadResult.Succeeded:
                    // Cache raw script
                    _rawScript = RawToLines(_scriptLoader!.Asset!.Split(Definitions.OSF_VALID_LINEENDING));

                    // Is this zero length?
                    if (_rawScript.Length == 0)
                    {
                        // Dip out
                        Debug.WriteLine("Zero length script loaded, cancelling conversation.");
                        EndChat();
                        break;
                    }

                    // Now cache line markers
                    _lineMarkers = GenLineMarkers(_rawScript);

                    // And now cache version info
                    (_scriptGame, _scriptVersion) = GenScriptVersion(_rawScript);

                    // Log potential issues
                    (string oysterGame, string oysterVer) = GetVersionNumberAndName();
                    if (oysterGame != _scriptGame) Debug.WriteLine($"Warning! Script game and Oyster game do not match, some script commands may not be supported (Oyster: {oysterGame}, Script: {_scriptGame})!");
                    if (oysterVer != _scriptVersion) Debug.WriteLine($"Warning! Script version and Oyster version do not match, some script commands may either be unsupported or function differently than expected (Oyster: {oysterVer}, Script: {_scriptVersion})!");

                    // Now using this we can initialise an array for storing lines
                    _script = new ISpeechCommand[_rawScript.Length];

                    // Set our indices
                    _currentCommandIndex = 0;
                    _nextCommandToLoadIndex = 0;

                    // Now load our first command
                    _safeToLoadMoreCommands = true;
                    LoadNextCommand();

                    // Set ourself to talking
                    _oysterState = SpeechState.Talking;

                    // Invoke script loaded event
                    if (OnScriptReady != null) OnScriptReady();
                    break;

                // No
                case A_BackgroundAssetLoader<string>.LoadResult.Failed:
                default:
                    // Then log issue and end conversation
                    Debug.WriteLine(log);
                    EndChat();
                    break;
            }

            // Now that we're done we should clean up the script loading stuff
            _scriptLoader = null;
        }
        /// <summary>
        /// Cleans up after a conversation.
        /// </summary>
        private static void EndChat()
        {
            // Tell the player's speech display to hide itself
            if (_playerScript != null) _playerScript.SpeechDisplay.Hide();

            // And now tell the scene script to reset itself
            if (_sceneScript != null) _sceneScript.ShowObjectsPostChat();

            // Now null everything out
            _playerScript = null;
            _characterScript = null;
            _sceneScript = null;
            _rawScript = null;
            _lineMarkers = null;

            // Reset script to an empty array
            _script = Array.Empty<ISpeechCommand>();

            // And set state back
            _oysterState = SpeechState.NotTalking;
        }
        /// <summary>
        /// Fetches the version number and name for this build of Oyster.
        /// </summary>
        private static (string name, string number) InternalNumberAndName() { return (Definitions.VERSION_NAME_STRING, Definitions.VERSION_NUMBER_STRING); }
        /// <summary>
        /// Runs a single tick of logic for Oyster.
        /// </summary>
        private static void Tick()
        {
            // What state are we in?
            switch (_oysterState)
            {
                // In a conversation?
                case SpeechState.Talking:
                    Tick_Talk();
                    break;
            }
        }
        private static void Tick_Talk()
        {
            // Are we at the end of the conversation?
            if (_currentCommandIndex >= _script.Length)
            {
                // Raise event
                if (OnSpeechCompleted != null) OnSpeechCompleted();

                // End the chat
                EndChat();

                // Stop what's about to happen
                return;
            }

            // Is the current line loaded?
            if (_script[_currentCommandIndex] == null)
            {
                // If not then load it and update the loader
                _nextCommandToLoadIndex = _currentCommandIndex;
                LoadNextCommand();
            }

            // Is the current line a text pusher, plus is it done
            if ((_script[_currentCommandIndex] is ITextPusher) &&
                !(_script[_currentCommandIndex] as ITextPusher)!.HasCharactersRemaining)
            {
                // Is the done counter... well... done?
                if (_promptWaitTimer > Definitions.PROMPT_WAIT_TIME)
                {
                    // Show done prompt
                    _playerScript!.SpeechDisplay.ContinuePrompt.Show();
                }

                // Otherwise we can just bump the counter
                else _promptWaitTimer++;
            }

            // If not, then hide prompt and reset counter
            else { _playerScript!.SpeechDisplay.ContinuePrompt.Hide(); _promptWaitTimer = 0; }

            // Otherwise we should process the current line
            if (_script[_currentCommandIndex]!.Run()) _currentCommandIndex++;

            // Is it safe to load more lines?
            if (_safeToLoadMoreCommands)
            {
                // Then load another!
                LoadNextCommand();
            }
        }
        private static void UnloadAllCommands()
        {
            // Iterate every command and then set them to null
            for (int i = 0; i < _script.Length; i++) _script[i] = null;
        }

        // Public Methods
        /// <summary>
        /// Attempts to start a chat with the given parameters.
        /// </summary>
        /// <param name="sceneScript">The script for the game's current scene.</param>
        /// <param name="playerTalker">The player's conversation script.</param>
        /// <param name="characterTalker">The character's conversation script.</param>
        /// <returns>True if a conversation is set to be loaded, false if anything went wrong.</returns>
        public static bool StartChat(
            A_SceneScript sceneScript,
            A_PlayerTalker playerTalker,
            A_CharacterTalker characterTalker
            )
        {
            // Ensure these are actual things
            if (sceneScript == null || playerTalker == null || characterTalker == null) { Debug.WriteLine("At least one parameter to StartChat() was null."); return false; }

            // Pass these values across for later use
            _sceneScript = sceneScript;
            _playerScript = playerTalker;
            _characterScript = characterTalker;
            Debug.WriteLine("Cached all relevant scripts for conversation.");

            // TODO: Figure out how to implement the 'lookers'

            // Tell the scene script to hide anything it needs to
            _sceneScript.HideObjectsForChat();

            // Tell the player script to reset its display
            _playerScript.SpeechDisplay.ResetDisplay();

            // And now we should update the display using the character info
            _playerScript.SpeechDisplay.NameText.Text = _characterScript.Data.DisplayName;
            _playerScript.SpeechDisplay.NameText.TextColour = _characterScript.Data.DisplayNameColour;
            Debug.WriteLine("Finished prepping speech display.");

            // Now let's show the speech box since it's all set-up
            _playerScript.SpeechDisplay.Show();
            Debug.WriteLine("Told the player to show their speech display.");

            // Begin loading the script for this conversation
            _scriptLoader = _characterScript.Data.BeginScriptLoad();

            // Set self to loading state
            _scriptLoader.OnLoadFinished += OnScriptLoaded;
            _oysterState = SpeechState.Loading;
            _scriptLoader.BeginAssetLoad();
            Debug.WriteLine("Loading script for conversation.");
            return true;
        }
        /// <summary>
        /// Fetches the version number and name of this Oyster runtime. Set the 'OnVersionGetRequest' event to return custom values. This method is used for logging potential incompatibility with scripts.
        /// </summary>
        public static (string name, string number) GetVersionNumberAndName()
        {
            // Raise event if subbed
            if (OnVersionGetRequest != null) return OnVersionGetRequest();

            // Otherwise use default
            return (Definitions.VERSION_NAME_STRING, Definitions.VERSION_NUMBER_STRING);
        }
        /// <summary>
        /// Lets Oyster know that this command exists for when it loads scripts.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        /// <param name="creationMethod">The method that should be called to create an instance of the command.</param>
        public static void AddCommand(string name, CommandCreationDelegate creationMethod)
        {
            // Cache this
            name = name.ToLower();

            // Overwrite if exists
            if (_validCommands.ContainsKey(name)) { _validCommands[name] = creationMethod; return; }

            // Otherwise add value
            _validCommands.Add(name, creationMethod);
        }
        /// <summary>
        /// Runs the next frame of logic for Oyster.
        /// </summary>
        /// <param name="deltaTime">The time since the last call to this method.</param>
        public static void Update(float deltaTime)
        {
            // Add time
            _timeSinceLastFrame += deltaTime;

            // Loop until we've caught up
            while (_timeSinceLastFrame > Definitions.SECONDS_PER_TICK)
            {
                // Call tick
                Tick();

                // Decrement time
                _timeSinceLastFrame -= Definitions.SECONDS_PER_TICK;
            }
        }
        /// <summary>
        /// Nudges Oyster. Imagine it like this, the user wants the conversation to continue, so the user says to Oyster "Hey, can you speed this up?", and so Oyster says, "Yeah sure!".
        /// </summary>
        public static void Nudge()
        {
            // Are we in a conversation? If not then dip.
            if (_oysterState != SpeechState.Talking) return;

            // Since we're in a conversation tell current line to go faster if it can
            if (_script[_currentCommandIndex] is ITakesTime) (_script[_currentCommandIndex] as ITakesTime)!.MakeItGoFaster();
        }
        /// <summary>
        /// Given anything is subbed to OnPuppetCalled, invokes it with the given command name.
        /// </summary>
        public static void CallPuppet(string command) { if (OnPuppetCalled != null) OnPuppetCalled(command); }
        public static void JumpToLineMarker(string lineMarkerName)
        {
            // Line markers will always be a valid dictionary when called via a command.
            // So we can skip a null check here

            // Does the line marker exist?
            if (_lineMarkers!.ContainsKey(lineMarkerName))
            {
                // Given it does, we should use the index to jump
                _currentCommandIndex = _lineMarkers[lineMarkerName];

                // Now we need to ensure the loader can do its job
                // No need to move it, the update tick checks for if the command is null (which will be ensured next)
                _safeToLoadMoreCommands = true;

                // Now null out every command
                UnloadAllCommands();
            }
        }

        // Accessors
        /// <summary>
        /// Gets whether Oyster has finished initialising.
        /// </summary>
        public static bool Loaded { get { return _loaded; } }
        /// <summary>
        /// Gets or sets the current script that Oyster is working with.
        /// </summary>
        public static A_SceneScript? SceneScript { get { return _sceneScript; } set { _sceneScript = value; } }
        /// <summary>
        /// Gets or sets Oyster's reference to the player's speech script.
        /// </summary>
        public static A_PlayerTalker? PlayerTalker { get { return _playerScript; } set { _playerScript = value; } }
        /// <summary>
        /// Gets or sets Oyster's reference to the character's speech script.
        /// </summary>
        public static A_CharacterTalker? CharacterTalker { get { return _characterScript; } set { _characterScript = value; } }
    }
}
