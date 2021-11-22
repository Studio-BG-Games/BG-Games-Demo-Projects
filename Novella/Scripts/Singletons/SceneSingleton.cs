using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// This class implements the Singleton pattern.
    /// Instance is only available in the call scene.
    /// </summary>
    /// <typeparam name="T">Inheritance class.</typeparam>
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
    {
        public static T Instance;

        public virtual void Awake()
        {
            if (Instance == null)
                Instance = (T)this;
            else
                Destroy(gameObject);
        }
    }
}