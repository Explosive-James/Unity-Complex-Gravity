using ComplexGravity.DataTypes;
using ComplexGravity.Utilities;
using UnityEngine;

namespace ComplexGravity
{
    public abstract class GravityField : MonoBehaviour
    {
        #region Data
#if UNITY_EDITOR
        [Header("Debugging")]
        [SerializeField] bool _displayBounds = false;
        [SerializeField] Color _displayColour = ColourExtensions.GetRandomColour();
#endif
        #endregion

        #region Properties
        /// <summary>
        /// What physics layers should the gravity field interact with.
        /// </summary>
        [field: Header("Physics Settings")]
        [field: SerializeField]
        public LayerMask PhysicsLayer { get; private set; } = int.MaxValue;
        /// <summary>
        /// Should rigidbodies inside the gravity field ignore global gravity.
        /// </summary>
        [field: SerializeField]
        public bool IgnoreGlobalGravity { get; private set; } = true;

        /// <summary>
        /// Higher priorities prevent lower priority gravity fields from influencing a rigidbody.
        /// </summary>
        [field: Header("Gravity Settings")]
        [field: SerializeField]
        public int Priority { get; private set; } = 0;
        /// <summary>
        /// The maximum force to apply to a rigidbody.
        /// </summary>
        [field: SerializeField]
        public GravityStrength GravityStrength { get; private set; } = new GravityStrength(9.81f);

        /// <summary>
        /// The range of the gravity field.
        /// </summary>
        [field: Header("Size Settings")]
        [field: SerializeField]
        public FadedRange GravityRange { get; private set; } = new FadedRange(0, 5, 0, 1);
        #endregion

        #region Unity Functions
        protected virtual void OnEnable()
        {
            GravityManager.Instance.RegisterGravityField(this);
        }
        protected virtual void OnDisable()
        {
            GravityManager.Instance.DeregisterGravityField(this);
        }
        protected virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (_displayBounds) {

                Bounds bounds = GetGlobalBounds();

                Gizmos.matrix = Matrix4x4.TRS(bounds.center, transform.rotation, Vector3.one);
                Gizmos.color = new Color(.69f, 1, .35f, 1);

                Gizmos.DrawWireCube(Vector3.zero, bounds.size);
                Gizmos.matrix = Matrix4x4.identity;
            }
#endif

            Gizmos.color = _displayColour;
        }
        #endregion

        #region Abstract Functions
        /// <summary>
        /// The world position and global size of the gravity field's influence.
        /// </summary>
        /// <returns></returns>
        public abstract Bounds GetGlobalBounds();
        /// <summary>
        /// The world-space gravity force being applied at a given position.
        /// </summary>
        public abstract Vector3 GetGlobalGravityForce(Vector3 globalPosition);
        #endregion
    }
}
