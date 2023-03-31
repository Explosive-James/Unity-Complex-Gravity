using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class CubicGravity : GravityField
    {
        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = transform.localToWorldMatrix;

            float maximumFade = (GravityRange.MaximumRange - GravityRange.MaximumFade) * 2;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(maximumFade, maximumFade, maximumFade));

            float minimumFade = (GravityRange.MinimumRange + GravityRange.MinimumFade) * 2;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(minimumFade, minimumFade, minimumFade));

            Gizmos.color = Gizmos.color.SetAlpha(.5f);

            float maximumRange = GravityRange.MaximumRange * 2;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(maximumRange, maximumRange, maximumRange));

            float minimumRange = GravityRange.MinimumRange * 2;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(minimumRange, minimumRange, minimumRange));

            float max = maximumRange * .5f;
            float min = minimumRange * .5f;

            Gizmos.DrawLine(new Vector3(min, min, min), new Vector3(max, max, max));
            Gizmos.DrawLine(new Vector3(-min, min, min), new Vector3(-max, max, max));
            Gizmos.DrawLine(new Vector3(min, -min, min), new Vector3(max, -max, max));
            Gizmos.DrawLine(new Vector3(min, min, -min), new Vector3(max, max, -max));
            Gizmos.DrawLine(new Vector3(-min, -min, min), new Vector3(-max, -max, max));
            Gizmos.DrawLine(new Vector3(-min, min, -min), new Vector3(-max, max, -max));
            Gizmos.DrawLine(new Vector3(min, -min, -min), new Vector3(max, -max, -max));
            Gizmos.DrawLine(new Vector3(-min, -min, -min), new Vector3(-max, -max, -max));
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            Vector3 localScale = new Vector3(GravityRange.MaximumRange,
                GravityRange.MaximumRange, GravityRange.MaximumRange);
            Vector3 globalScale = Vector3.Scale(transform.lossyScale, localScale);

            return new Bounds(transform.position, globalScale * 2);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            Vector3 absoluteScale = transform.lossyScale.Absolute();
            Vector3 difference = globalPosition - transform.position;

            difference = Quaternion.Inverse(transform.rotation) *
                VectorExtensions.Divide(difference, absoluteScale);

            Vector3 absoluteDifference = difference.Absolute();
            float maximumDifference = Mathf.Max(absoluteDifference.x, absoluteDifference.y, absoluteDifference.z);

            Vector3 direction;

            if (maximumDifference == absoluteDifference.x) {
                direction = transform.right * Mathf.Sign(-difference.x);
            }
            else if (maximumDifference == absoluteDifference.y) {
                direction = transform.up * Mathf.Sign(-difference.y);
            }
            else {
                direction = transform.forward * Mathf.Sign(-difference.z);
            }
            return GravityStrength.Value * GravityRange.Evaluate(maximumDifference) * direction;
        }
        #endregion
    }
}
