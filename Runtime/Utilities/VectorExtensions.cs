using UnityEngine;
using System.Runtime.CompilerServices;

namespace ComplexGravity.Utilities
{
    public static class VectorExtensions
    {
        #region Public Functions
        /// <summary>
        /// Gets the abolsute values of a Vector3
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Absolute(this Vector3 vector3)
        {
            return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
        }
        /// <summary>
        /// Divides the x,y,z of one vector3 with another, x/x,y/y,z/z.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Divide(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x / vector2.x, vector1.y / vector2.y, vector1.z / vector2.z);
        }
        #endregion
    }
}
