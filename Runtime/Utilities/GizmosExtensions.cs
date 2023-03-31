using UnityEngine;

namespace ComplexGravity.Utilities
{
    public static class GizmosExtensions
    {
        #region Public Functions
        public static void DrawWireArc(Vector3 position, Vector3 normal, float angle, float radius, int resolution = 32)
        {
            Quaternion rotation = Quaternion.LookRotation(normal);
            Vector3 previousPosition = position + rotation * new Vector3(0, radius, 0);

            resolution = Mathf.Max(resolution, 1);

            for (int i = 0; i < resolution; i++) {

                float degrees = (i + 1f) / resolution * angle;
                float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
                float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

                Vector3 currentPosition = position + rotation * new Vector3(sin * radius, cos * radius, 0);
                Gizmos.DrawLine(previousPosition, currentPosition);

                previousPosition = currentPosition;
            }
        }
        public static void DrawWireCylinder(Vector3 position, Vector3 direction, float length, float radius, int resolution = 32)
        {
            Vector3 p1 = position + (.5f * length * direction);
            Vector3 p2 = position + (.5f * length * -direction);

            DrawWireArc(p1, direction, 360, radius, resolution);
            DrawWireArc(p2, direction, 360, radius, resolution);

            Quaternion rotation = Quaternion.LookRotation(direction);

            Vector3 localUpwards = rotation * new Vector3(0, radius, 0);

            Gizmos.DrawLine(p1 + localUpwards, p2 + localUpwards);
            Gizmos.DrawLine(p1 - localUpwards, p2 - localUpwards);

            Vector3 localRightward = rotation * new Vector3(radius, 0, 0);

            Gizmos.DrawLine(p1 + localRightward, p2 + localRightward);
            Gizmos.DrawLine(p1 - localRightward, p2 - localRightward);
        }
        public static void DrawWireHemisphere(Vector3 position, Vector3 normal, float radius, int resolution = 32)
        {
            DrawWireArc(position, normal, 360, radius, resolution);
            int halfResolution = Mathf.Max(resolution / 2, 1);

            Quaternion rotation = Quaternion.LookRotation(normal);

            Vector3 previousPoint1 = position + rotation * new Vector3(0, radius, 0);
            Vector3 previousPoint2 = position + rotation * new Vector3(radius, 0, 0);

            for (int i = 0; i < halfResolution; i++) {

                float degrees = 90 + (i + 1f) / halfResolution * 180;
                float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
                float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

                Vector3 currentPoint1 = position + rotation * new Vector3(0, sin * radius, cos * radius);
                Vector3 currentPoint2 = position + rotation * new Vector3(sin * radius, 0, cos * radius);

                Gizmos.DrawLine(previousPoint1, currentPoint1);
                Gizmos.DrawLine(previousPoint2, currentPoint2);

                previousPoint1 = currentPoint1;
                previousPoint2 = currentPoint2;
            }
        }
        public static void DrawWireCapsule(Vector3 position, Vector3 direction, float length, float radius, int resolution = 32)
        {
            Vector3 p1 = position + (direction * length);
            Vector3 p2 = position + (-direction * length);

            DrawWireHemisphere(p1, -direction, radius, resolution);
            DrawWireHemisphere(p2, direction, radius, resolution);

            Quaternion rotation = Quaternion.LookRotation(direction);

            Vector3 localUpward = rotation * Vector3.up * radius;

            Gizmos.DrawLine(p1 + localUpward, p2 + localUpward);
            Gizmos.DrawLine(p1 - localUpward, p2 - localUpward);

            Vector3 localRightward = rotation * Vector3.right * radius;

            Gizmos.DrawLine(p1 + localRightward, p2 + localRightward);
            Gizmos.DrawLine(p1 - localRightward, p2 - localRightward);
        }
        public static void DrawWireTorus(Vector3 position, Vector3 direction, float radius, float thickness, int resolution = 32)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);

            Vector3 forwards = rotation * Vector3.up;
            Vector3 upwards = rotation * Vector3.forward;
            Vector3 rightwards = rotation * Vector3.right;

            DrawWireArc(position + (forwards * radius), rightwards, 360, thickness, resolution);
            DrawWireArc(position - (forwards * radius), rightwards, 360, thickness, resolution);
            DrawWireArc(position + (rightwards * radius), forwards, 360, thickness, resolution);
            DrawWireArc(position - (rightwards * radius), forwards, 360, thickness, resolution);

            DrawWireArc(position + (upwards * thickness), upwards, 360, radius, resolution);
            DrawWireArc(position - (upwards * thickness), upwards, 360, radius, resolution);
            DrawWireArc(position, upwards, 360, radius - thickness, resolution);
            DrawWireArc(position, upwards, 360, radius + thickness, resolution);
        }
        #endregion
    }
}
