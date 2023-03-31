using ComplexGravityEditor.Settings.DataTypes;
using UnityEngine;

namespace ComplexGravityEditor.Settings
{
    [System.Serializable]
    internal class SettingsContainer : ScriptableObject
    {
        #region Data
        public GravitySettings settings;
        #endregion

        #region Constructor
        public static SettingsContainer CreateInstance(GravitySettings settings)
        {
            SettingsContainer container = CreateInstance<SettingsContainer>();
            container.settings = settings;

            return container;
        }
        #endregion
    }
}
