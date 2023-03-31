using System;
using UnityEngine;

namespace ComplexGravity.DataTypes
{
    /// <summary>
    /// Creates a linear dropoff for a value within a given range.
    /// </summary>
    [Serializable]
    public struct FadedRange
    {
        #region Data
        [SerializeField, HideInInspector] 
        float _minimumRange, _maximumRange;
        [SerializeField, HideInInspector] 
        float _minimumFade, _maximumFade;
        #endregion

        #region Properties
        /// <summary>
        /// Values smaller than this will evaluate to 0.
        /// </summary>
        public float MinimumRange => _minimumRange;
        /// <summary>
        /// Values higher than this will evaluate to 0.
        /// </summary>
        public float MaximumRange => _maximumRange;
        /// <summary>
        /// The distance from the MinimumRange where the value will start lerping to 0.
        /// </summary>
        public float MinimumFade => _minimumFade;
        /// <summary>
        /// The distance from the MaximumRange where the value will start lerping to 0.
        /// </summary>
        public float MaximumFade => _maximumFade;
        #endregion

        #region Constructor
        public FadedRange(float minimumRange, float maximumRange, float minimumFade, float maximumFade)
        {
            if (minimumRange > maximumRange) {
                throw new ArgumentException("minimumRange is greater than maximumRange");
            }

            _minimumRange = minimumRange;
            _maximumRange = maximumRange;

            if(minimumFade < 0 || maximumFade < 0) {
                throw new ArgumentException("minimumFade and maximumFade must be greater than 0");
            }

            float fadeDelta = maximumRange - minimumRange;

            if(minimumFade > fadeDelta - maximumFade) {
                throw new ArgumentException($"minimumFade must be {fadeDelta - maximumFade} or less");
            }
            if(maximumFade > fadeDelta - minimumFade) {
                throw new ArgumentException($"maximumFade must be {fadeDelta - minimumFade} or less");
            }

            _minimumFade = minimumFade;
            _maximumFade = maximumFade;
        }
        #endregion

        #region Public Functions
        public float Evaluate(float value)
        {
            float maximumFactor = Mathf.Clamp01(-(value - _maximumRange) / _maximumFade);
            float minimumFactor = Mathf.Clamp01((value - _minimumRange) / _minimumFade);

            if (float.IsNaN(maximumFactor)) return minimumFactor;
            if (float.IsNaN(minimumFactor)) return maximumFactor;

            return maximumFactor * minimumFactor;
        }
        #endregion
    }
}
