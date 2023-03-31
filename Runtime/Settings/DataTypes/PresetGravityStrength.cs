#if UNITY_EDITOR

namespace ComplexGravityEditor.Settings.DataTypes
{
    /// <summary>
    /// Couples a gravity strength with a name.
    /// </summary>
    [System.Serializable]
    public struct PresetGravityStrength
    {
        #region Data
        /// <summary>
        /// Name of the preset.
        /// </summary>
        public string name;
        /// <summary>
        /// Strength value of the preset.
        /// </summary>
        public float strength;
        #endregion

        #region Constructor
        public PresetGravityStrength(string name, float strength)
        {
            this.name = name;
            this.strength = strength;
        }
        #endregion
    }
}
#endif
