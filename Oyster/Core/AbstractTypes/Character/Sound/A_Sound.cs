namespace Oyster.Core.AbstractTypes.Character.Sound
{
    public abstract class A_Sound
    {
        // Protected Variables
        protected string _name;
        protected object _sound;

        // Constructor
        public A_Sound(string name, object sound)
        {
            // Pass Values
            _name = name;
            _sound = sound;
        }

        // Accessors
        /// <summary>
        /// Gets or sets the name of this sound effect.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        /// <summary>
        /// Gets or sets an object representing the sound that this object holds.
        /// </summary>
        public object Sound { get { return _sound; } set { _sound = value; } }
    }
}
