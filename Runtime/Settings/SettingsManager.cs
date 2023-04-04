#if UNITY_EDITOR
using ComplexGravityEditor.Settings.DataTypes;
using ComplexGravity.DataTypes;
using System;
using System.IO;
using UnityEngine;

namespace ComplexGravityEditor.Settings
{
    /// <summary>
    /// Manages the global settings for the complex gravity package.
    /// </summary>
    public static class SettingsManager
    {
        #region Data
        static GravitySettings _settings;
        #endregion

        #region Properties
        public static GravitySettings Settings {
            get {
                return _settings ??= LoadSettings();
            }
        }
        #endregion

        #region Public Functions
        public static void SaveSettings()
        {
            string directory = GetFilePath();
            string payloadText = JsonUtility.ToJson(_settings);

            File.WriteAllText(directory, payloadText);
        }
        public static GravitySettings LoadSettings()
        {
            string directory = GetFilePath();

            if (File.Exists(directory)) {

                string payloadText = File.ReadAllText(directory);

                return JsonUtility.FromJson<GravitySettings>(payloadText);
            }

            return new GravitySettings() {

                physicsLayers = int.MaxValue,
                ignoreGlobalGravity = true,
                gravityStrength = new GravityStrength(9.81f),

                strengths = new PresetGravityStrength[] {
                    new PresetGravityStrength("Low", 4.90f),
                    new PresetGravityStrength("Normal", 9.81f),
                    new PresetGravityStrength("High", 19.62f),
                }
            };
        }
        #endregion

        #region Private Functions
        private static string GetFilePath()
        {
            return Path.Combine(Environment.CurrentDirectory,
                "ProjectSettings\\GravitySettings.asset");
        }
        #endregion
    }
}
#endif
