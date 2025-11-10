using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Debug/Log", fileName = "Log")]
public class Log : ScriptableObject
{
    public static void PrintLog(string message)
    {
        Debug.Log(message);
    }

    public static void PrintLog(Collider coll)
    {
        Debug.Log(coll.name);
    }
}
