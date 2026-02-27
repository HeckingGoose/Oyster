using Oyster.Core.AbstractTypes.Scene;
using Oyster.Core.Interfaces.Things;

namespace Oyster.Core.AbstractTypes.Player
{
    public abstract class A_Camera : ICamera
    {
        // Protected Variables
        protected A_Looker? _lookTarget;
        protected A_Looker? _npcLookTarget;
        protected A_Looker _playerLookTarget;

        // Constructor
        public A_Camera(A_Looker playerTarget)
        {
            // Pass Value
            _playerLookTarget = playerTarget;

            // And set current looker to player's
            _lookTarget = _playerLookTarget;

            // Null other
            _npcLookTarget = null;
        }

        // Public Methods
        /// <summary>
        /// Changes the look target for this camera via its name.
        /// </summary>
        public void SetLookTargetByName(string targetName)
        {
            // Iterate through look targets in scene script
            if (OysterMain.SceneScript != null)
            {
                foreach (A_Looker looker in OysterMain.SceneScript.Lookers)
                {
                    // Does the name match?
                    if (looker.Name == targetName)
                    {
                        // Set looker
                        _lookTarget = looker;
                    }
                }
            }
        }
        /// <summary>
        /// Sets the NPC looker of this camera to the given target.
        /// </summary>
        public void SetNPCLookTarget(A_Looker? lookTarget) { _npcLookTarget = lookTarget; }
        /// <summary>
        /// Resets this camera's look target to its current NPC target. Given that target is not null.
        /// </summary>
        public void ResetLookTarget_ToNPC()
        {
            // Null check
            if (_npcLookTarget == null) return;

            // Reset look target to NPC target
            _lookTarget = _npcLookTarget;
        }
        /// <summary>
        /// Resets this camera's look target match the player's facing direction.
        /// </summary>
        public void ResetLookTarget_ToPlayer()
        {
            // Reset look target to player target
            _lookTarget = _playerLookTarget;
        }

        // Accessors
        public abstract int FOV { get; set; }
        /// <summary>
        /// Gets or sets the current look target for this camera.
        /// </summary>
        public A_Looker LookTarget
        {
            get { if (_lookTarget == null) _lookTarget = _playerLookTarget; return _lookTarget; }
            set { _lookTarget = value; }
        }

    }
}
