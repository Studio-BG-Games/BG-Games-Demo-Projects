using UnityEngine;

namespace Scripts
{
    /// <summary>
    /// This class implements the Singleton pattern.
    /// Instance available throughout the game.
    /// </summary>
    /// <typeparam name="T">Inheritance class.</typeparam>
    public class GameSingleton<T> : MonoBehaviour where T : GameSingleton<T>
    {
        public static T Instance;

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = (T) this;
                if (Application.isPlaying)
                    DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}