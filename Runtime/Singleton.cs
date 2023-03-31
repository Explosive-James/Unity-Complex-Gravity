using UnityEngine;

namespace ComplexGravity
{
    /// <summary>
    /// A type that implements the singleton pattern.
    /// </summary>
    /// <typeparam name="T">The type inheriting the singleton.</typeparam>
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        #region Data
        private static T _instance;
        private static bool _destroying;
        #endregion

        #region Properties
        /// <summary>
        /// The instance of the singleton.
        /// </summary>
        public static T Instance {
            get {
                if (_instance == null && !_destroying) {
                    _instance = CreateSingletonInstance();
                }
                return _instance;
            }
        }
        #endregion

        #region Unity Functions
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this) {

                Destroy(this);
            }
            else {

                _instance = (T)this;
                DontDestroyOnLoad(this);
            }
        }
        protected virtual void OnApplicationQuit()
        {
            _destroying = true;
        }
        #endregion

        #region Private Functions
        private static T CreateSingletonInstance()
        {
            T[] instances = FindObjectsOfType<T>();

            /* If the Awake function is overridden that doesn't call the base version it would allow multiple 
             * instances to exist even if only one if officially recognised as the singleton instance.*/
            if (instances.Length > 0) {

                for (int i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);

                return instances[0];
            }

            GameObject gameObject = new GameObject(typeof(T).Name);
            return gameObject.AddComponent<T>();
        }
        #endregion
    }
}
