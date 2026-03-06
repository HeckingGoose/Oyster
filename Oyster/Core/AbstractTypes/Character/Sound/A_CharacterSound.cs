using System;
using System.Collections.Generic;

namespace Oyster.Core.AbstractTypes.Character.Sound
{
    public abstract class A_CharacterSound
    {
        // Protected Variables
        protected A_SoundPlayer _soundPlayer;
        protected Dictionary<string, A_Sound> _sounds;

        // Constructors
        public A_CharacterSound(A_SoundPlayer soundPlayer) : this(soundPlayer, Array.Empty<A_Sound>()) { }
        public A_CharacterSound(A_SoundPlayer soundPlayer, A_Sound[] sounds)
        {
            // Pass Value
            _soundPlayer = soundPlayer;

            // Init dict
            _sounds = new Dictionary<string, A_Sound>();

            // Add to dict
            foreach (A_Sound sound in sounds)
            {
                AddSound(sound);
            }
        }

        // Private Methods
        /// <summary>
        /// Called when a sound has finished loading. Specifically called from 'PlaySound'.
        /// </summary>
        private void OnSoundLoaded(A_BackgroundAssetLoader<A_Sound>.LoadResult loadResult, A_Sound? sound, string log)
        {
            // Was it a success? Yes.
            if (loadResult == A_BackgroundAssetLoader<A_Sound>.LoadResult.Succeeded)
            {
                // Check null
                if (sound == null)
                {
                    // Log and exit
                    DebugOut.Warn("Loaded sound is null!");
                    return;
                }

                // Read back sound
                _sounds.Add(sound.Name, sound);

                // Now play it!
                PlaySound(sound);
            }
            // Nope!
            else
            {
                // Log it
                DebugOut.Warn($"Failed to load sound! Error: {log}");
            }
        }

        // Protected Methods
        /// <summary>
        /// Plays a sound.
        /// </summary>
        /// <param name="sound">The sound to play.</param>
        protected void PlaySound(A_Sound sound)
        {
            // Reset playback time.
            _soundPlayer.Stop();

            // Set clip
            _soundPlayer.Sound = sound;

            // Now play
            _soundPlayer.Play();
        }
        /// <summary>
        /// Adds the given sound to be stored within this class.
        /// </summary>
        /// <param name="sound">The named sound to store.</param>
        protected void AddSound(A_Sound sound)
        {
            // Index sound by name
            _sounds.Add(sound.Name, sound);
        }
        /// <summary>
        /// Begins loading the sound of the given name.
        /// </summary>
        /// <param name="name">The name of the sound to load.</param>
        /// <returns>A background asset loader, that on a successful load will contain the requested sound.</returns>
        protected abstract A_BackgroundAssetLoader<A_Sound> LoadSound(string name);

        // Public Methods
        /// <summary>
        /// Plays the sound of the given name. If the sound does not exist, does nothing.
        /// </summary>
        public void PlaySound(string soundName)
        {
            // Does this sound exist? If not then load it.
            if (!_sounds.ContainsKey(soundName))
            {
                // Load and wait
                A_BackgroundAssetLoader<A_Sound> handle = LoadSound(soundName);
                handle.OnLoadFinished = OnSoundLoaded;
                return;
            }

            // Sound exists, so play it
            PlaySound(_sounds[soundName]);
        }
    }
}
