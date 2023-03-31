using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class CapsuleGravity : GravityField
    {
        #region Properties
        [field: SerializeField, Min(0)]
        public float Length { get; private set; } = 10;
        #endregion

        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = transform.localToWorldMatrix;

            float length = Mathf.Max(Length, GravityRange.MaximumRange) - GravityRange.MaximumRange;

            GizmosExtensions.DrawWireCapsule(Vector3.zero, Vector3.forward, length, GravityRange.MaximumRange - GravityRange.MaximumFade);
            GizmosExtensions.DrawWireCapsule(Vector3.zero, Vector3.forward, length, GravityRange.MinimumRange + GravityRange.MinimumFade);

            Gizmos.color = Gizmos.color.SetAlpha(.5f);

            GizmosExtensions.DrawWireCapsule(Vector3.zero, Vector3.forward, length, GravityRange.MaximumRange);
            GizmosExtensions.DrawWireCapsule(Vector3.zero, Vector3.forward, length, GravityRange.MinimumRange);
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            float localLength = Mathf.Max(Length, GravityRange.MaximumRange);

            Vector3 localScale = new Vector3(GravityRange.MaximumRange, GravityRange.MaximumRange, localLength);
            Vector3 globalScale = Vector3.Scale(localScale, transform.lossyScale);

            return new Bounds(transform.position, globalScale * 2);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            float localLength = Mathf.Max(Length - GravityRange.MaximumRange, 0);

            Vector3 localPosition = transform.InverseTransformPoint(globalPosition);
            localPosition.z -= Mathf.Min(Mathf.Abs(localPosition.z), localLength) * Mathf.Sign(localPosition.z);

            float localDistance = localPosition.magnitude;
            Vector3 direction = -transform.TransformVector(localPosition).normalized;

            return GravityStrength.Value * GravityRange.Evaluate(localDistance) * direction;
        }
        #endregion
    }
}
