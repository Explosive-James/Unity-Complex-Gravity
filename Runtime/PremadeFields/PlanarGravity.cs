using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public sealed class PlanarGravity : GravityField
    {
        #region Properties
        [field: SerializeField, Min(0)]
        public Vector2 Dimensions { get; private set; } = new Vector2(5, 5);
        #endregion

        #region Unity Functions
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.matrix = transform.localToWorldMatrix;

            float rangeHeight = GravityRange.MaximumRange - GravityRange.MinimumRange;
            float rangeOffset = (rangeHeight * .5f) + GravityRange.MinimumRange;

            float fadeHeight = rangeHeight - GravityRange.MaximumFade - GravityRange.MinimumFade;
            float fadeOffset = (fadeHeight * .5f) + GravityRange.MinimumFade + GravityRange.MinimumRange;

            Gizmos.DrawWireCube(new Vector3(0, fadeOffset, 0), new Vector3(Dimensions.x * 2, fadeHeight, Dimensions.y * 2));
            Gizmos.color = Gizmos.color.SetAlpha(.5f);
            Gizmos.DrawWireCube(new Vector3(0, rangeOffset, 0), new Vector3(Dimensions.x * 2, rangeHeight, Dimensions.y * 2));
        }
#endif
        #endregion

        #region Abstract Functions
        public override Bounds GetGlobalBounds()
        {
            Vector3 localScale = new Vector3(Dimensions.x * 2, GravityRange.MaximumRange, Dimensions.y * 2);

            Vector3 globalScale = Vector3.Scale(localScale, transform.lossyScale);
            Vector3 globalPosition = transform.position + (transform.rotation * new Vector3(0, globalScale.y * .5f, 0));

            return new Bounds(globalPosition, globalScale);
        }
        public override Vector3 GetGlobalGravityForce(Vector3 globalPosition)
        {
            Vector3 localPosition = transform.InverseTransformPoint(globalPosition);

            if (localPosition.x < -Dimensions.x || localPosition.x > Dimensions.x ||
                localPosition.z < -Dimensions.y || localPosition.z > Dimensions.y) {

                return Vector3.zero;
            }

            return GravityStrength.Value * GravityRange.Evaluate(localPosition.y) * 
                (transform.rotation * Vector3.up);
        }
        #endregion
    }
}
