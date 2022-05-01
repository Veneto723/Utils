using UnityEngine;

namespace Utils.Scripts.Runtime.ObjectPool {
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
        public static T Instance { get; private set; }

        protected virtual void Awake() {
            // if (Instance != null) Debug.Log($"{name} has already benn initialized!");

            Instance = (T) this;
        }
    }
}