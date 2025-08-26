using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.Controls;

namespace GameUI
{
    public class KeyDisplayUI : MonoBehaviour
    {
        [SerializeField]   private TMP_Text keyDisplayText;

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                keyDisplayText.text = "";
                return;
            }

            string pressedKeys = "";
            foreach (KeyControl key in keyboard.allKeys)
            {
                if (key == null)
                    continue;
                if (key.isPressed)
                {
                    string name = key.displayName ?? "";
                    if (name.Length > 0)
                        pressedKeys += name + " ";
                }
            }

            keyDisplayText.text = pressedKeys.Trim();
        }
    }
}