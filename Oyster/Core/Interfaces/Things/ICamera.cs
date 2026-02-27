namespace Oyster.Core.Interfaces.Things
{
    public interface ICamera
    {
        // Accessors
        /// <summary>
        /// Gets or sets the field of view for this camera in integer degrees.
        /// </summary>
        public int FOV { get; set; }
    }
}
