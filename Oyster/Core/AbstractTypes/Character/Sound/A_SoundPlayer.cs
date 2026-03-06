namespace Oyster.Core.AbstractTypes.Character.Sound
{
    public abstract class A_SoundPlayer
    {
        // Public Methods
        /// <summary>
        /// Begins playing the current sound from its current progress.
        /// </summary>
        public abstract void Play();
        /// <summary>
        /// Stops playback of the current sound and resets playback time to zero.
        /// </summary>
        public abstract void Stop();
        /// <summary>
        /// Stops playback of the current sound without resetting playback time.
        /// </summary>
        public abstract void Pause();

        // Accessors
        /// <summary>
        /// Gets or sets the current sound attached to this source. Also resets playback progress to zero.
        /// </summary>
        public abstract A_Sound Sound { get; set; }
        /// <summary>
        /// Gets whether this source is currently playing.
        /// </summary>
        public abstract bool IsPlaying { get; }
        /// <summary>
        /// Gets or sets whether this source is set to loop.
        /// </summary>
        public abstract bool IsLooping { get; set; }
        /// <summary>
        /// Gets or sets the current progress through the sound in seconds.
        /// </summary>
        public abstract float Time { get; set; }
        /// <summary>
        /// Gets the seconds length of the sound currently associated with this source.
        /// </summary>
        public abstract float Length { get; }
    }
}
