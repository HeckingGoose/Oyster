namespace Oyster.ImplementationInterfaces
{
    public interface ICamera
    {
        // Accessors
        /// <summary>
        /// Gets or sets the field of view for this camera.
        /// </summary>
        public float FOV { get; set; }
        /// <summary>
        /// Gets or sets the name of the object that the camera should look at during conversation.
        /// </summary>
        public string LookTargetName { get; set; }
    }
}
