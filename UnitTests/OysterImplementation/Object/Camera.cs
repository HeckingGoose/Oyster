using Oyster.Core.Interfaces.Things;

namespace UnitTests.OysterImplementation.Object
{
    internal class Camera : ICamera
    {
        // Const
        private const string DEFAULT_LOOKTARGET = "default";

        // Private Variables
        private int _fov;
        private string _lookTarget;
        private string _baseLookTarget;

        // Constructor
        public Camera(int fov)
        {
            // Pass Value
            _fov = fov;

            // Set default
            _lookTarget = DEFAULT_LOOKTARGET;
            _baseLookTarget = DEFAULT_LOOKTARGET;
        }

        // Public Methods
        public void ResetLookTarget()
        {
            // Reset to base
            _lookTarget = _baseLookTarget;
        }

        public void SetDefaultLookTarget(string targetName)
        {
            // Set base target
            _baseLookTarget = targetName;
        }

        // Accessors
        public int FOV
        {
            get { return _fov; }
            set { _fov = value; }
        }
        public string LookTargetName
        {
            get { return _lookTarget; }
            set { _lookTarget = value; }
        }
        public string BaseLookTarget { get { return _baseLookTarget; } }
    }
}
