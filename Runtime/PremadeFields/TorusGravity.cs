using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class TorusGravity : GravityField
    {
        #region Properties
        [field: SerializeField, Min(0)]
        public float Radius { get; private set; } = 5;
        #endregion

        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = transform.localToWorldMatrix;

            float radius = Mathf.Max(Radius, GravityRange.MaximumRange);

            GizmosExtensions.DrawWireTorus(Vector3.zero, Vector3.up, radius, GravityRange.MaximumRange - GravityRange.MaximumFade);
            GizmosExtensions.DrawWireTorus(Vector3.zero, Vector3.up, radius, GravityRange.MinimumRange + GravityRange.MinimumFade);

            Gizmos.color = Gizmos.color.SetAlpha(.5f);

            GizmosExtensions.DrawWireTorus(Vector3.zero, Vector3.up, radius, GravityRange.MaximumRange);
            GizmosExtensions.DrawWireTorus(Vector3.zero, Vector3.up, radius, GravityRange.MinimumRange);
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            float height = Mathf.Max(Radius, GravityRange.MaximumRange);
            float width = (height + GravityRange.MaximumRange) * 2;

            Vector3 localScale = new Vector3(width, height * 2, width);
            Vector3 globalScale = Vector3.Scale(localScale, transform.lossyScale);

            return new Bounds(transform.position, globalScale);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(globalPosition);
            float radius = Mathf.Max(Radius, GravityRange.MaximumRange);

            Vector3 torusPoint = new Vector3(localPosition.x, 0, localPosition.z).normalized * radius;

            Vector3 localDirection = torusPoint - localPosition;
            float distance = localDirection.magnitude;

            Vector3 direction = transform.TransformVector(localDirection).normalized;
            return GravityStrength.Value * GravityRange.Evaluate(distance) * direction;
        }
        #endregion
    }
}
