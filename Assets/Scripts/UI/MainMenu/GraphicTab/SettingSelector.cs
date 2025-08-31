using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Image[] _selectSlots;

    [Header("Sprites")]
    [SerializeField] private Sprite _selectedOn;
    [SerializeField] private Sprite _selectedOff;

    private string[] _options;
    private int _currentIndex = 0;
    private int _initialIndex = 0;
    private int _previousIndex = 0; 
  
    public void Init(string[] options, int defaultIndex)
    {
        _options = options;
        _initialIndex = defaultIndex;
        _currentIndex = defaultIndex;
        _previousIndex = defaultIndex;

        UpdateUI();
        HookEvents();
    }

    private void HookEvents()
    {
        _leftButton.onClick.AddListener(() =>
        {
            _previousIndex = _currentIndex;
            _currentIndex = (_currentIndex - 1 + _options.Length) % _options.Length;
            UpdateUI();
        });

        _rightButton.onClick.AddListener(() =>
        {
            _previousIndex = _currentIndex;
            _currentIndex = (_currentIndex + 1) % _options.Length;
            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        _valueText.text = _options[_currentIndex];

        _selectSlots[_previousIndex].sprite = _selectedOff;
        _selectSlots[_currentIndex].sprite = _selectedOn;
    }
       
    public int Apply()
    {
        _initialIndex = _currentIndex;

        return _initialIndex;
    }

    public void Revert()
    {
        _previousIndex = _currentIndex;
        _currentIndex = _initialIndex;
        UpdateUI();
    }
}
