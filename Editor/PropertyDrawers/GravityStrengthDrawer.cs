using ComplexGravity.DataTypes;
using ComplexGravityEditor.Settings;
using ComplexGravityEditor.Settings.DataTypes;
using UnityEditor;
using UnityEngine;

namespace ComplexGravityEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(GravityStrength))]
    internal class GravityStrengthDrawer : PropertyDrawer
    {
        #region Unity Functions
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.height = EditorGUIUtility.singleLineHeight;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            SerializedProperty presetProperty = property.FindPropertyRelative("_preset");
            SerializedProperty strengthProperty = property.FindPropertyRelative("_strength");
            SerializedProperty invertProperty = property.FindPropertyRelative("_invert");

            GravitySettings settings = SettingsManager.Settings;
            string[] displayNames = GetPresetNames(settings);

            if (presetProperty.intValue > settings.strengths.Length) {
                presetProperty.intValue = 0;
            }

            presetProperty.intValue = EditorGUI.Popup(position, presetProperty.intValue, displayNames);
            position.y += EditorGUIUtility.singleLineHeight * 1.1f;

            if(presetProperty.intValue != 0) {
                strengthProperty.floatValue = settings.strengths[presetProperty.intValue - 1].strength;
            }

            EditorGUI.BeginDisabledGroup(presetProperty.intValue != 0);
            EditorGUI.PropertyField(position, strengthProperty, GUIContent.none);

            EditorGUI.EndDisabledGroup();
            position.y += EditorGUIUtility.singleLineHeight * 1.1f;

            EditorGUI.PropertyField(position, invertProperty, new GUIContent("Invert Strength"));

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2.3f;
        }
        #endregion

        #region Private Functions
        private string[] GetPresetNames(GravitySettings settings)
        {
            string[] results = new string[settings.strengths.Length + 1];
            results[0] = "Custom";

            for (int i = 0; i < results.Length - 1; i++)
                results[i + 1] = settings.strengths[i].name;

            return results;
        }
        #endregion
    }
}
