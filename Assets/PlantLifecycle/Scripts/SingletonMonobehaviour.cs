using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TMKOC.PlantLifecycle
{
    public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        // private static readonly object _lock = new object();   // Not needed in Unity, since it is single-threaded, but interesting to know.
        private static bool _applicationIsQuitting = false;

        [Header("This is a Singleton Monobehaviour")]
        public bool isPersistent = true;


        /// <summary>
        /// Gets the Singleton instance of the class.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance of {typeof(T)} is already destroyed. Returning null.");
                    return null;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();

                        //do we need to do this here???
                        // DontDestroyOnLoad(singletonObject);


                    }
                }

                return _instance;

            }
        }

        /// <summary>
        /// Ensures that the Singleton is properly initialized.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (isPersistent)
                    DontDestroyOnLoad(gameObject);

            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Marks the application as quitting to avoid accessing destroyed instances.
        /// </summary>
        protected virtual void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }

        /// <summary>
        /// Cleans up when the instance is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }




    }
}
