
using Oyster.Types;

namespace Oyster.ImplementationInterfaces
{
    public interface ICamera
    {
        // Accessors
        public float FOV { get; set; }
        public Vector3 LookAt { get; set; }
    }
}
