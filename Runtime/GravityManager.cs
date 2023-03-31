using ComplexGravity.DataTypes;
using ComplexGravity.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ComplexGravity
{
    /// <summary>
    /// Controls every gravity field interaction and can be used to query a rigidbody's current gravity.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GravityManager : Singleton<GravityManager>
    {
        #region Data
        private readonly Collider[] _collisionBuffer = new Collider[256];
        private readonly List<GravityField> _gravityFields = new List<GravityField>();

        private Dictionary<Rigidbody, GravityInfo> _gravityInfos = new Dictionary<Rigidbody, GravityInfo>();
        #endregion

        #region Unity Functions
        private void FixedUpdate()
        {
            foreach (GravityField gravityField in _gravityFields) {

                Bounds globalBounds = gravityField.GetGlobalBounds();
                globalBounds.size = globalBounds.size.Absolute() * .5f;

                int collisionCount = Physics.OverlapBoxNonAlloc(globalBounds.center, globalBounds.size,
                    _collisionBuffer, gravityField.transform.rotation, gravityField.PhysicsLayer);

                for (int i = 0; i < collisionCount; i++) {

                    Rigidbody rigidbody = _collisionBuffer[i].attachedRigidbody;

                    if (rigidbody == null || !rigidbody.useGravity || (_gravityInfos.TryGetValue(rigidbody, 
                        out GravityInfo gravityInfo) && gravityInfo.priority > gravityField.Priority)) {

                        continue;
                    }

                    Vector3 gravityForce = gravityField.GetGlobalGravityForce(rigidbody.position);

                    if (gravityForce != Vector3.zero) {

                        rigidbody.velocity += gravityForce * Time.fixedDeltaTime;

                        if (gravityInfo == null) {
                            _gravityInfos.Add(rigidbody, new GravityInfo(gravityField, gravityForce));
                        }
                        else {
                            gravityInfo.UpdateRigidbodyInfo(gravityField, gravityForce);
                        }
                    }
                }
            }

            /* Used to count how many rigidbodys in the dictionary are null to prevent a memory leak,
             * when Unity "destroys" an object it reads as null but still exists in memory waiting for the 
             * GC to come along, if it is still being reference it will be ignored and stay in memory.*/
            int cleanupCount = 0;

            foreach (Rigidbody rigidbody in _gravityInfos.Keys) {

                if (rigidbody == null) {

                    cleanupCount++;
                    continue;
                }

                GravityInfo rigidbodyInfo = _gravityInfos[rigidbody];

                if (rigidbody.useGravity) {

                    /* A gravity field will mark a rigidbody when it wants to remove global gravity,
                     * because multiple gravity fields can interact with the same rigidbody they 
                     * can't handle that force themselves so it is instead handled here.*/
                    if (rigidbodyInfo.ignoreGlobalGravity) {

                        rigidbody.velocity -= Physics.gravity * Time.fixedDeltaTime;
                        rigidbodyInfo.ignoreGlobalGravity = false;
                    }
                    else {
                        rigidbodyInfo.previousGravity += Physics.gravity;
                    }
                }

                /* Checking if the rigidbody whent a frame without being interacted with by a gravity 
                 * field as that means it has left whatever field it was inside of and now needs the 
                 * priority reset so lower priority fields can interact with it again.*/
                if (rigidbodyInfo.previousFrame - Time.frameCount != 0) {

                    rigidbodyInfo.priority = int.MinValue;
                    rigidbodyInfo.previousGravity = rigidbody.useGravity ? Physics.gravity : Vector3.zero;
                }
            }

            if (cleanupCount > 10) {

                Dictionary<Rigidbody, GravityInfo> gravityInfo = new Dictionary<Rigidbody, GravityInfo>(_gravityInfos.Count);

                foreach (KeyValuePair<Rigidbody, GravityInfo> keyValuePair in _gravityInfos)
                    if (keyValuePair.Key != null) {
                        gravityInfo.Add(keyValuePair.Key, keyValuePair.Value);
                    }

                _gravityInfos = gravityInfo;
            }
        }
        #endregion

        #region Internal Functions
        internal void RegisterGravityField(GravityField gravityField)
        {
            _gravityFields.Add(gravityField);
            /* Because gravity fields cannot communicate with one another it's possible for a lower 
             * priority field to apply gravity before a higher priority resulting in incorrect gravity, 
             * the collection must therefore be sorted so higher priority fields are called first.*/
            _gravityFields.Sort((a, b) => b.Priority - a.Priority);
        }
        internal void DeregisterGravityField(GravityField gravityField)
        {
            _gravityFields.Remove(gravityField);
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gets the gravity being applied to a given rigidbody from all gravity sources.
        /// </summary>
        public Vector3 GetRigidbodyGravity(Rigidbody rigidbody)
        {
            if (_gravityInfos.TryGetValue(rigidbody, out GravityInfo info)) {
                return info.previousGravity;
            }
            return rigidbody.useGravity ? Physics.gravity : Vector3.zero;
        }
        /// <summary>
        /// Gets the gravity at a given position from all gravity sources.
        /// </summary>
        public Vector3 GetGravityAtPosition(Vector3 position)
        {
            int priority = int.MinValue;
            bool ignoreGlobalGravity = false;

            Vector3 globalGravity = Vector3.zero;

            foreach (GravityField gravity in _gravityFields) {

                /* Since the gravity fields are sorted by priority, if we find a lower priority field 
                 * then we know there will no longer be any more fields that can apply gravity.*/
                if (gravity.Priority < priority) break;

                Vector3 gravityForce = gravity.GetGlobalGravityForce(position);

                if (gravityForce != Vector3.zero) {

                    globalGravity += gravityForce;
                    priority = gravity.Priority;
                    ignoreGlobalGravity = ignoreGlobalGravity || gravity.IgnoreGlobalGravity;
                }
            }

            if (!ignoreGlobalGravity) {
                globalGravity += Physics.gravity;
            }
            return globalGravity;
        }
        #endregion
    }
}
