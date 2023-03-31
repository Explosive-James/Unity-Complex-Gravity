using UnityEngine;
#if UNITY_EDITOR
using ComplexGravityEditor.Settings;
#endif

namespace ComplexGravity.DataTypes
{
    /// <summary>
    /// Represents the strength of a gravity field.
    /// </summary>
    [System.Serializable]
    public class GravityStrength : ISerializationCallbackReceiver
    {
        #region Data
        [SerializeField, HideInInspector] int _preset = 0;
        [SerializeField, HideInInspector] float _strength = 9.81f;
        #endregion

        #region Properties
        public float Value {
            get {
                return _strength;
            }
        }
        #endregion

        #region Constructor
        public GravityStrength(float strength)
        {
            _strength = strength;
        }
        #endregion

        #region Interface Functions
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR

            if (_preset == 0) {
                return;
            }

            /* If the global settings are changed the _strength value won't update until it is drawn in the inspector again 
             * however this should always match the global values. But OnBeforeSerialize gets called on all instances 
             * including during a build which gives us one last chance to update the variable to the correct value.*/
            if (SettingsManager.Settings.presetStrengths.Length > _preset - 1) {
                _strength = SettingsManager.Settings.presetStrengths[_preset - 1].strength;
            }
#endif
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
        #endregion
    }
}
