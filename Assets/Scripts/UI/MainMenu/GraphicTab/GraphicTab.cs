using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class GraphicTab : MonoBehaviour, ISettingsPage<GraphicSettingData>
    {
        public event Action OnSettingChanged;

        [Header("Setting Selectors")]
        [SerializeField] private SettingSelector _displayModeSelector;
        [SerializeField] private SettingSelector _resolutionSelector;
        [SerializeField] private SettingSelector _performanceSelector;

        private readonly string[] _displayModes = { "전체화면", "창 모드", "테두리 없음" };
        private readonly string[] _resolutions = { "1920×1080", "1600×900", "1280×720" };
        private readonly string[] _performanceOptions = { "하", "중", "상" };

        GraphicSettingData _data;


        public void Init(GraphicSettingData data)
        {
            _data = data;
            _displayModeSelector.Init(_displayModes, _data.DisplayModeIndex);
            _resolutionSelector.Init(_resolutions, _data.ResolutionIndex);
            _performanceSelector.Init(_performanceOptions, _data.PerformanceIndex);

            RegisterChangeEvents();
        }

        public void LoadSetting()
        {
            _displayModeSelector.Revert();
            _resolutionSelector.Revert();
            _performanceSelector.Revert();
        }

        public void ApplySetting()
        {
            _data.DisplayModeIndex = _displayModeSelector.Apply();
            _data.ResolutionIndex = _resolutionSelector.Apply();
            _data.PerformanceIndex = _performanceSelector.Apply();
            OnSettingChanged?.Invoke();
        }

        private void RegisterChangeEvents()
        {
            _displayModeSelector.GetComponentInChildren<Button>().onClick.AddListener(() => OnSettingChanged?.Invoke());
            _resolutionSelector.GetComponentInChildren<Button>().onClick.AddListener(() => OnSettingChanged?.Invoke());
            _performanceSelector.GetComponentInChildren<Button>().onClick.AddListener(() => OnSettingChanged?.Invoke());
        }

        public GraphicSettingData GetSubData()
        {
            return _data;
        }
    }
}