using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class SphericalGravity : GravityField
    {
        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, transform.lossyScale);

            Gizmos.DrawWireSphere(Vector3.zero, GravityRange.MaximumRange - GravityRange.MaximumFade);
            Gizmos.DrawWireSphere(Vector3.zero, GravityRange.MinimumRange + GravityRange.MinimumFade);

            Gizmos.color = Gizmos.color.SetAlpha(.5f);

            Gizmos.DrawWireSphere(Vector3.zero, GravityRange.MaximumRange);
            Gizmos.DrawWireSphere(Vector3.zero, GravityRange.MinimumRange);
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            Vector3 globalScale = 2 * GravityRange.MaximumRange * transform.lossyScale;
            return new Bounds(transform.position, globalScale);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            Vector3 localPosition = transform.position - globalPosition;
            localPosition = VectorExtensions.Divide(localPosition, transform.lossyScale);

            float distance = localPosition.magnitude;
            return GravityStrength.Value * GravityRange.Evaluate(distance) * (localPosition / distance);
        }
        #endregion
    }
}
