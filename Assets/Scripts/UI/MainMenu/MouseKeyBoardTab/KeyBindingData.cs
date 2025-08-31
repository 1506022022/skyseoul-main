using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "DefaultKeyBindingData",
    menuName = "Settings/Key Binding Data", 
    order = 0
)]
public class KeyBindingData : ScriptableObject
{
    [Serializable]
    public class KeyBind
    {
        public string actionName;
        public KeyCode key;
    }

    public List<KeyBind> keyBinds = new List<KeyBind>();

    public void SetKey(string actionName, KeyCode newKey)
    {
        var bind = keyBinds.Find(k => k.actionName == actionName);
        if (bind != null)
        {
            bind.key = newKey;
        }
    }

    public KeyCode GetKey(string actionName)
    {
        var bind = keyBinds.Find(k => k.actionName == actionName);
        return bind != null ? bind.key : KeyCode.None;
    }
}
