using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour, Singleton<MonoSingleton<T>> where T : MonoSingleton<T>
{

    protected virtual void Awake()
    {
        if (Singleton<MonoSingleton<T>>._instance != null)
        {
            Debug.LogWarning($"Singleton: Existing instance of {typeof(T).FullName} found! This will be replaced with another instance!");
        }
        Singleton<MonoSingleton<T>>._instance = this;
    }

}