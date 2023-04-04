using UnityEngine;
using UnityEditor;
using ComplexGravity;
using ComplexGravityEditor.Settings;
using ComplexGravityEditor.Settings.DataTypes;

namespace ComplexGravityEditor
{
    public static class GravityContextMenu
    {
        #region Menu Functions
        [MenuItem("GameObject/GravityFields/Cubic Gravity")]
        static void CreateCubicGravity(MenuCommand command) => CreateGravityField<CubicGravity>(command);

        [MenuItem("GameObject/GravityFields/Cylindrical Gravity")]
        static void CreateCylindricalGravity(MenuCommand command) => CreateGravityField<CylindricalGravity>(command);

        [MenuItem("GameObject/GravityFields/Capsule Gravity")]
        static void CreateCapsuleGravity(MenuCommand command) => CreateGravityField<CapsuleGravity>(command);

        [MenuItem("GameObject/GravityFields/Planar Gravity")]
        static void CreatePlanarGravity(MenuCommand command) => CreateGravityField<PlanarGravity>(command);

        [MenuItem("GameObject/GravityFields/Spherical Gravity")]
        static void CreateSphericalGravity(MenuCommand command) => CreateGravityField<SphericalGravity>(command);

        [MenuItem("GameObject/GravityFields/Torus Gravity")]
        static void CreateTorusGravity(MenuCommand command) => CreateGravityField<TorusGravity>(command);
        #endregion

        #region Public Functions
        public static void CreateGravityField<T>(MenuCommand command)
            where T : GravityField
        {
            GameObject gravityField = new GameObject(typeof(T).Name);
            GameObjectUtility.SetParentAndAlign(gravityField, command.context as GameObject);

            if (gravityField.transform.parent == null) {
                gravityField.transform.position = GetSpawnPosition();
            }

            GravitySettings settings = SettingsManager.Settings;
            GravityField field = gravityField.AddComponent<T>();

            field.PhysicsLayer = settings.physicsLayers;
            field.IgnoreGlobalGravity = settings.ignoreGlobalGravity;
            field.GravityStrength = settings.gravityStrength;

            Undo.RegisterCreatedObjectUndo(gravityField, $"Create {gravityField.name}");
            Selection.activeGameObject = gravityField;
        }
        #endregion

        #region Private Functions
        private static Vector3 GetSpawnPosition()
        {
            Camera[] activeCameras = SceneView.GetAllSceneCameras();

            if (activeCameras.Length == 0) return Vector3.zero;

            Vector3 origin = activeCameras[0].transform.position;
            Vector3 direction = activeCameras[0].transform.forward;

            float raycastDistance = activeCameras[0].nearClipPlane * 100;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, raycastDistance)) {

                return hit.point;
            }
            return origin + (direction * raycastDistance);
        }
        #endregion
    }
}
