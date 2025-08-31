using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace GameUI
{
   
    public class KeyBinder : MonoBehaviour
    {
        [Header("Button Fields")]
        [SerializeField] private Button _keyButton1;

        [Header("Text Fields")]
        [SerializeField] private TMP_Text _label;

        [Header("Input Action")]
        [SerializeField] private InputActionReference _actionReference;

        [Tooltip("Composite part: Up, Down, Left, Right")]
        [SerializeField] private CompositePart _compositePart;

        [Header("Delay Setting")]
        [Range(0, 1)]
        [SerializeField] private float _blinkDelay = 0.5f;

        private bool _isWaiting = false;
        private Coroutine _cursorCoroutine;
        private string _currentBinding;
        private string _pendingBinding;

        private void Start()
        {
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            if (_actionReference?.action == null)
            {
                Debug.LogWarning($"{nameof(KeyBinder)}: InputActionReference is not assigned.");
                SetLabel(" ");
                return;
            }

            if (_keyButton1 != null)
                _keyButton1.onClick.AddListener(StartRebinding);

            UpdateBindingDisplay();
        }


        private void StartRebinding()
        {
            if (_isWaiting || _actionReference?.action == null)
                return;

            _isWaiting = true;
            _cursorCoroutine = StartCoroutine(BlinkingCursor());

            InputSystem.onAnyButtonPress.CallOnce(control =>
            {
                _isWaiting = false;
                if (_cursorCoroutine != null)
                    StopCoroutine(_cursorCoroutine);

                if (IsEscapeKey(control))
                {
                    UpdateBindingDisplay();
                    return;
                }


                _pendingBinding = control.path;
                SetLabel(InputControlPath.ToHumanReadableString(_pendingBinding, InputControlPath.HumanReadableStringOptions.OmitDevice));
            });
        }


        public void ApplyPendingBinding()
        {
            if (string.IsNullOrEmpty(_pendingBinding))
                return;

            var action = _actionReference.action;
            int bindingIndex = GetCompositeBindingIndex();

            if (bindingIndex >= 0)
            {
                action.ChangeBinding(bindingIndex).WithPath(_pendingBinding);
                _currentBinding = _pendingBinding;
                _pendingBinding = null;
            }
        }
        public void ResetToDefault(KeyCode defaultKey)
        {
            if (_actionReference?.action == null)
                return;

            int bindingIndex = GetCompositeBindingIndex();
            if (bindingIndex >= 0)
            {

                string keyPath = $"<Keyboard>/{defaultKey.ToString().ToLower()}";

                _actionReference.action.ChangeBinding(bindingIndex).WithPath(keyPath);
                _currentBinding = keyPath;
                _pendingBinding = null;
                UpdateBindingDisplay();
            }
        }

        public void CancelPendingBinding()
        {
            _pendingBinding = null;
            UpdateBindingDisplay();
        }

        private bool IsEscapeKey(InputControl control)
        {
            return control.device is Keyboard &&
                   control is KeyControl keyControl &&
                   keyControl.keyCode == Key.Escape;
        }

        private void UpdateBindingDisplay()
        {
            var action = _actionReference.action;
            int bindingIndex = GetCompositeBindingIndex();

            if (bindingIndex >= 0)
            {
                var binding = action.bindings[bindingIndex];
                _currentBinding = binding.effectivePath;
                SetLabel(InputControlPath.ToHumanReadableString(_currentBinding, InputControlPath.HumanReadableStringOptions.OmitDevice));
            }
            else
            {
                SetLabel(" ");
            }
        }

        private int GetCompositeBindingIndex()
        {
            var bindings = _actionReference.action.bindings;
            if (_compositePart == CompositePart.None)
            {
                for (int i = 0; i < bindings.Count; i++)
                {
                    if (!bindings[i].isComposite && !bindings[i].isPartOfComposite)
                        return i;
                }

                Debug.LogError($"No single binding found for {_actionReference.action.name}");
                return -1;
            }
            string partName = _compositePart.ToString().ToLower();
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].isPartOfComposite && bindings[i].name == partName)
                    return i;
            }

            Debug.LogError($"Binding part '{partName}' not found in {_actionReference.action.name}");
            return -1;
        }
        private void SetLabel(string text)
        {
            _label.text = text.ToUpper();
        }

        private IEnumerator BlinkingCursor()
        {
            bool visible = true;
            WaitForSeconds delay = new WaitForSeconds(_blinkDelay);
            while (_isWaiting)
            {
                _label.text = visible ? "|" : "";
                visible = !visible;
                yield return delay;
            }
        }
    }

    public enum CompositePart
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}