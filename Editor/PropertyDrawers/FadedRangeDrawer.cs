using ComplexGravity.DataTypes;
using UnityEditor;
using UnityEngine;

namespace ComplexGravityEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(FadedRange))]
    internal class FadedRangeDrawer : PropertyDrawer
    {
        #region Unity Functions
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(position, new GUIContent(label.text + ":"));
            EditorGUI.indentLevel++;

            SerializedProperty minimumRangeProp = property.FindPropertyRelative("_minimumRange");
            SerializedProperty maximumRangeProp = property.FindPropertyRelative("_maximumRange");
            SerializedProperty minimumFadeProp = property.FindPropertyRelative("_minimumFade");
            SerializedProperty maximumFadeProp = property.FindPropertyRelative("_maximumFade");

            float maximumFadeDelta = maximumRangeProp.floatValue - minimumRangeProp.floatValue;

            position.y += EditorGUIUtility.singleLineHeight * 1.25f;

            EditorGUI.PropertyField(position, minimumRangeProp);
            minimumRangeProp.floatValue = Mathf.Clamp(minimumRangeProp.floatValue, 0, maximumRangeProp.floatValue);

            position.y += EditorGUIUtility.singleLineHeight * 1.2f;

            float minimumFadeValue = EditorGUI.Slider(position, new GUIContent(minimumFadeProp.displayName),
                minimumFadeProp.floatValue, 0, maximumFadeDelta);
            minimumFadeProp.floatValue = Mathf.Clamp(minimumFadeValue, 0, maximumFadeDelta - maximumFadeProp.floatValue);

            position.y += EditorGUIUtility.singleLineHeight * 1.2f;

            EditorGUI.PropertyField(position, maximumRangeProp);
            maximumRangeProp.floatValue = Mathf.Max(minimumRangeProp.floatValue, maximumRangeProp.floatValue);

            position.y += EditorGUIUtility.singleLineHeight * 1.2f;

            float maximumFadeValue = EditorGUI.Slider(position, new GUIContent(maximumFadeProp.displayName),
                maximumFadeProp.floatValue, 0, maximumFadeDelta);
            maximumFadeProp.floatValue = Mathf.Clamp(maximumFadeValue, 0, maximumFadeDelta - minimumFadeProp.floatValue);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 6f;
        }
        #endregion
    }
}
