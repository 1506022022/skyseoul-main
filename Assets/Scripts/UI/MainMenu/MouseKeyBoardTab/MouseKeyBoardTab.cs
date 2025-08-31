using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindingData;

namespace GameUI
{
    public class MouseKeyBoardTab : MonoBehaviour, ISettingsPage<MouseKeyBoardData>
    {
        [Header("Mouse Sensivity Setting")]
        [SerializeField] private Slider _mouseSensivity;
        [SerializeField] private TextMeshProUGUI _sensivityValue;

        [Header("KeyBoard TextFields")]
        [SerializeField] private List<KeyBinder> _keyBoardFields;

        [Header("Default Key Setting")]
        [SerializeField] private Button _defaultKeyButton;
        [SerializeField] private KeyBindingData _defaultKeyBindingData;

        private int _mouseValue = 0;
        private MouseKeyBoardData _data;

        public event Action OnSettingChanged;


        private void OnEnable()
        {
            _mouseSensivity.onValueChanged.AddListener(_ => OnChangeValue());
            _defaultKeyButton.onClick.AddListener(ResetKeyData);
        }

        public void OnChangeValue()
        {
            int sensivity = (int)_mouseSensivity.value;
            _data.MouseSensivity = sensivity;
            _mouseValue = sensivity;
            _sensivityValue.text = _mouseValue.ToString();

        }

        private void ResetKeyData()
        {
            if (_defaultKeyBindingData)
            {
                List<KeyBind> keyBinds = _defaultKeyBindingData.keyBinds;
                for (int i = 0; i < _keyBoardFields.Count; i++)
                {
                    _keyBoardFields[i].ResetToDefault(keyBinds[i].key);
                }
            }
        }

        public void ApplySetting()
        {
            if (_data != null)
                _data.MouseSensivity = _mouseValue;

            for (int i = 0; i < _keyBoardFields.Count; i++)
            {
                _keyBoardFields[i].ApplyPendingBinding();
            }

            OnSettingChanged?.Invoke();
        }

        public void Init(MouseKeyBoardData data)
        {
            _data = data;
            LoadSetting();
        }

        public void LoadSetting()
        {
            int value = _data.MouseSensivity;
            _mouseValue = value;

            _mouseSensivity.SetValueWithoutNotify(value);
            _sensivityValue.text = value.ToString();

        }

        public MouseKeyBoardData GetSubData()
        {
            return _data;
        }
    }
}