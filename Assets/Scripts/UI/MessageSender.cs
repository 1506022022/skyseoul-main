using UnityEngine;

[CreateAssetMenu(fileName = "MessageSender", menuName = "Scriptable Objects/MessageSender")]
public class MessageSender : ScriptableObject
{
    public void PrintMessage(string message)
    {
        RobotMessenger.Instance.PrintMessage(message);
    }
}
