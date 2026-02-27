namespace Oyster.Core.AbstractTypes.Scene
{
    public class A_Looker
    {
        // Protected Variables
        protected string _name;
        protected object _target;

        // Constructor
        public A_Looker(string name, object target)
        {
            // Pass Value
            _name = name;
            _target = target;
        }

        // Accessors
        /// <summary>
        /// Returns the name of this looker.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Returns the target that this looker represents. Type depends on engine integration.
        /// </summary>
        public object Target { get { return _target; } }
    }
}
