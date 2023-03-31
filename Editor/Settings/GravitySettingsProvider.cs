using System.Collections.Generic;
using UnityEditor;

namespace ComplexGravityEditor.Settings
{
    internal static class GravitySettingsProvider
    {
        static SerializedObject settings;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            SettingsProvider settingsProvider = new SettingsProvider("Project/Gravity Settings", SettingsScope.Project) {

                label = "Complex Gravity",
                keywords = new HashSet<string>(new[] { "Complex", "Gravity", "Strength", "Preset" }),

                guiHandler = (searchContext) => {

                    settings ??= new SerializedObject(SettingsContainer.CreateInstance(SettingsManager.Settings));
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.Separator();
                    EditorGUILayout.PropertyField(settings.FindProperty("settings"));

                    if (EditorGUI.EndChangeCheck()) {

                        settings.ApplyModifiedProperties();
                        SettingsManager.SaveSettings();
                    }
                }
            };

            return settingsProvider;
        }
    }
}
