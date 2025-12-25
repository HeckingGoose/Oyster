namespace Oyster.Core.Interfaces.Things
{
    public interface ICamera
    {
        // Accessors
        /// <summary>
        /// Gets or sets the field of view for this camera in integer degrees.
        /// </summary>
        public int FOV { get; set; }
        /// <summary>
        /// Gets or sets the name of the object that the camera should look at during conversation.
        /// </summary>
        public string LookTargetName { get; set; }
    }
}
