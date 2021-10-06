using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T staticInstance;

    public static T Instance
    {
        get
        {
            if (staticInstance is { })
            {
                return staticInstance;
            }
            staticInstance = FindObjectOfType(typeof(T)) as T;

            if (staticInstance is null)
            {
                Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
            }

            return staticInstance;
        }
    }
}