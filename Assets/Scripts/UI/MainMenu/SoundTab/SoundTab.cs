using NUnit.Framework.Internal;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{ 
public class SoundTab : MonoBehaviour, ISettingsPage<AudioData>
{
    [Header("Sliders")]
    [SerializeField] SoundSlider _total;
    [SerializeField] SoundSlider _bgm;
    [SerializeField] SoundSlider _sfx;

    [Header("BackGround Audio")]
    [SerializeField] SelectButton _selectedOn;
    [SerializeField] SelectButton _selectedOff;

    [Header("Select Sprite")]
    [SerializeField] Sprite _selectedOnSprite;
    [SerializeField] Sprite _selectedOffSprite;

    private AudioData _data;
    public event Action OnSettingChanged;

    private void OnEnable() { RegisterChangeEvent(); }

    #region Volume Change Callbacks
    private void RegisterChangeEvent()
    {
        _total.slider.onValueChanged.AddListener(OnTotalVolumeChanged);
        _bgm.slider.onValueChanged.AddListener(OnBGMVolumeChanged);
        _sfx.slider.onValueChanged.AddListener(OnSFXVolumeChanged);

        _selectedOn._button.onClick.AddListener(() => OnClickSelect(true));
        _selectedOff._button.onClick.AddListener(() => OnClickSelect(false));
    }
    private void OnTotalVolumeChanged(float value)
    {
        _total.UpdateText();
        _data.TotalVolume = _total.Value;
    }

    private void OnBGMVolumeChanged(float value)
    {
        _bgm.UpdateText();
        _data.BGMVolume = _bgm.Value;
    }

    private void OnSFXVolumeChanged(float value)
    {
        _sfx.UpdateText();
        _data.EffectVolume = _sfx.Value;
    }
    public void OnClickSelect(bool value)
    {
        _data.RunInBackGround = value;
        UpdateBackGroundUI();
    }
    private void UpdateBackGroundUI()
    {
        if (_data.RunInBackGround)
        {
            _selectedOn.ChangeSprite(_selectedOnSprite);
            _selectedOff.ChangeSprite(_selectedOffSprite);
        }
        else
        {
            _selectedOn.ChangeSprite(_selectedOffSprite);
            _selectedOff.ChangeSprite(_selectedOnSprite);
        }
    }

    #endregion

    #region ISettingsModule
    public void Init(AudioData data)
    {
        _data = data;
        LoadSetting();
  
    }

    public void LoadSetting()
    {
        _total.Value = _data.TotalVolume;
        _bgm.Value = _data.BGMVolume;
        _sfx.Value = _data.EffectVolume;
        
        UpdateBackGroundUI();
    }
        
    public void ApplySetting()
    {
        AudioManager.ApplySettings(_data);
        OnSettingChanged?.Invoke();
    }

    public AudioData GetSubData() { return _data; }
  
    #endregion



}
[System.Serializable]
public struct SelectButton
{
    public Button _button;
    public Image _selected;

    public void ChangeSprite(Sprite sprite)
    {
        _selected.sprite = sprite;
    }
}
[System.Serializable]
public struct SoundSlider
{
    public Slider slider;
    public TextMeshProUGUI text;

    public int Value
    {
        get => Mathf.RoundToInt(slider.value);
        set
        {
            slider.value = value;
            text.text = value.ToString();
        }
    }
    public void UpdateText()
    {
        text.text = Mathf.RoundToInt(slider.value).ToString();
    }
}

    }