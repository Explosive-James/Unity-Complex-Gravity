using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class CylindricalGravity : GravityField
    {
        #region Properties
        [field: SerializeField, Min(0)]
        public float Length { get; private set; } = 5;
        #endregion

        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = transform.localToWorldMatrix;

            GizmosExtensions.DrawWireCylinder(Vector3.zero, Vector3.forward, Length * 2, GravityRange.MaximumRange - GravityRange.MaximumFade);
            GizmosExtensions.DrawWireCylinder(Vector3.zero, Vector3.forward, Length * 2, GravityRange.MinimumRange + GravityRange.MinimumFade);

            Gizmos.color = Gizmos.color.SetAlpha(.5f);

            GizmosExtensions.DrawWireCylinder(Vector3.zero, Vector3.forward, Length * 2, GravityRange.MaximumRange);
            GizmosExtensions.DrawWireCylinder(Vector3.zero, Vector3.forward, Length * 2, GravityRange.MinimumRange);
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            Vector3 localScale = new Vector3(GravityRange.MaximumRange,
                GravityRange.MaximumRange, Length);
            Vector3 globalScale = Vector3.Scale(transform.lossyScale, localScale);

            return new Bounds(transform.position, globalScale * 2);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(globalPosition);
            localPosition.z -= Mathf.Min(Mathf.Abs(localPosition.z), Length) * Mathf.Sign(localPosition.z);

            if (localPosition.z != 0) return Vector3.zero;

            Vector3 globalDirection = transform.rotation * Vector3.Scale(localPosition, transform.lossyScale);
            float distance = new Vector2(localPosition.x, localPosition.y).magnitude;

            return GravityStrength.Value * GravityRange.Evaluate(distance) * -globalDirection.normalized;
        }
        #endregion
    }
}
