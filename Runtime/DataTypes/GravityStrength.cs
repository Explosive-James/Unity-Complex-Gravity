﻿using UnityEngine;
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
        [SerializeField, HideInInspector] bool _invert = false;
        #endregion

        #region Properties
        public float Value {
            get {
                return _invert ? -_strength : _strength;
            }
        }
        #endregion

        #region Constructor
        public GravityStrength(float strength)
        {
            _strength = strength;
        }
        public GravityStrength(float strength, bool invert)
        {
            _strength = strength;
            _invert = invert;
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
            if (SettingsManager.Settings.strengths.Length > _preset - 1) {
                _strength = SettingsManager.Settings.strengths[_preset - 1].strength;
            }
#endif
        }
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }
        #endregion
    }
}
