using UnityEngine;

namespace ComplexGravity.DataTypes
{
    /// <summary>
    /// Stores gravity information about a Rigidbody for the GravityManager.
    /// </summary>
    internal class GravityInfo
    {
        #region Data
        /// <summary>
        /// The current gravity field priority the rigidbody is in.
        /// </summary>
        public int priority;
        /// <summary>
        /// The last frame number the priority was evaluated.
        /// </summary>
        public int previousFrame;
        /// <summary>
        /// Does the rigidbody need to currently ignore global gravity.
        /// </summary>
        public bool ignoreGlobalGravity;

        /// <summary>
        /// The previously calculated force applied to the rigidbody.
        /// </summary>
        public Vector3 previousGravity;
        #endregion

        #region Constructor
        public GravityInfo(GravityField gravityField, Vector3 currentGravity)
        {
            priority = gravityField.Priority;
            ignoreGlobalGravity = gravityField.IgnoreGlobalGravity;

            previousFrame = Time.frameCount;
            previousGravity = currentGravity;
        }
        #endregion

        #region Public Functions
        public void UpdateRigidbodyInfo(GravityField gravityField, Vector3 currentGravity)
        {
            priority = gravityField.Priority;
            ignoreGlobalGravity = gravityField.IgnoreGlobalGravity;

            if(previousFrame == Time.frameCount) {

                previousGravity += currentGravity;
            }
            else {

                previousGravity = currentGravity;
                previousFrame = Time.frameCount;
            }
        }
        #endregion
    }
}
