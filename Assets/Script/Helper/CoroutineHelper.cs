using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper _instance;
    public static CoroutineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject helperObject = new("CoroutineHelper");
                _instance = helperObject.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(helperObject);
            }
            return _instance;
        }
    }
}