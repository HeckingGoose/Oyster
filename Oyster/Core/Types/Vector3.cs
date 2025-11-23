namespace Oyster.Core.Types
{
    public class Vector3 : Vector
    {
        // Private Variables
        private float _x;
        private float _y;
        private float _z;

        // Constructor
        public Vector3(float x, float y, float z)
        {
            // Pass Values
            _x = x;
            _y = y;
            _z = z;
        }

        // Accessors
        public float X { get { return _x; } set { _x = value; } }
        public float Y { get { return _y; } set { _y = value; } }
        public float Z { get { return _z; } set { _z = value; } }
    }
}
