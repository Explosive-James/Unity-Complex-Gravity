#if UNITY_EDITOR
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
        /// The preset gravity names and strengths for the GravityStrength type.
        /// </summary>
        public PresetGravityStrength[] presetStrengths;
        #endregion

        #region Constructor
        public GravitySettings(params PresetGravityStrength[] presetStrengths)
        {
            this.presetStrengths = presetStrengths;
        }
        #endregion
    }
}
#endif