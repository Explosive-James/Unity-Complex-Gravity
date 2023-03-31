using ComplexGravity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ComplexGravityEditor.Inspectors
{
    [CustomEditor(typeof(GravityField), true)]
    internal class GravityFieldEditor : Editor
    {
        #region Data
        static readonly Dictionary<string, int> _skipProperties = new Dictionary<string, int> {
            { nameof(Vector2), 2 }, { nameof(Vector3), 3 }, { nameof(Vector4), 4 }, { nameof(Quaternion), 4},
        };
        SerializedObject _gravityField;
        #endregion

        #region Unity Functions
        public override void OnInspectorGUI()
        {
            _gravityField ??= new SerializedObject(target);

            SerializedProperty property = _gravityField.GetIterator();
            SkipProperties(property, 1);

            while (property.NextVisible(true)) {

                EditorGUILayout.PropertyField(property);

                if (_skipProperties.ContainsKey(property.type))
                    SkipProperties(property, _skipProperties[property.type]);
            }

            if (_gravityField.hasModifiedProperties)
                _gravityField.ApplyModifiedProperties();
        }
        #endregion

        #region Private Functions
        private void SkipProperties(SerializedProperty property, int count)
        {
            for (int i = 0; i < count; ++i)
                if (!property.NextVisible(true)) {
                    return;
                }
        }
        #endregion
    }
}
