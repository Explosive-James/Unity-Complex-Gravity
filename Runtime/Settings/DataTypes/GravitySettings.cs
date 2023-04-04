#if UNITY_EDITOR
using ComplexGravity.DataTypes;
using UnityEngine;

namespace ComplexGravityEditor.Settings.DataTypes
{
    /// <summary>
    /// The project settings for the complex gravity types.
    /// </summary>
    [System.Serializable]
    public class GravitySettings
    {
        #region Data
        /// <summary>
        /// The default physics layers the gravity field will interact with.
        /// </summary>
        [Header("Default Field Settings:")]
        public LayerMask physicsLayers;
        /// <summary>
        /// The default ignore global gravity setting for the gravity field.
        /// </summary>
        public bool ignoreGlobalGravity;
        /// <summary>
        /// The default strength of a gravity field.
        /// </summary>
        public GravityStrength gravityStrength;

        /// <summary>
        /// The preset gravity names and strengths for the GravityStrength type.
        /// </summary>
        [Header("Preset Strength Settings:")]
        public PresetGravityStrength[] strengths;
        #endregion
    }
}
#endif
