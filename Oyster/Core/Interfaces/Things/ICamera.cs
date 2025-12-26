namespace Oyster.Core.Interfaces.Things
{
    public interface ICamera
    {
        // Public Methods
        /// <summary>
        /// Changes the name of the default look target for this camera.
        /// </summary>
        public void SetDefaultLookTarget(string targetName);
        /// <summary>
        /// Resets this camera's look target to its default target.
        /// </summary>
        public void ResetLookTarget();

        // Accessors
        /// <summary>
        /// Gets or sets the field of view for this camera in integer degrees.
        /// </summary>
        public int FOV { get; set; }
        /// <summary>
        /// Gets or sets the name of the object that the camera should look at during conversation.
        /// </summary>
        public string LookTargetName { get; set; }
        /// <summary>
        /// Gets the name of the default look target for this camera.
        /// </summary>
        public string BaseLookTarget { get; }
    }
}
