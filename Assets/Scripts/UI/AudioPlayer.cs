using Games.System.Settings;
using System;
using UnityEditor.Build.Pipeline;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;


public class AudioPlayer : MonoBehaviour
{
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer _mixer;

    private const string MASTER_VOL = "Master";
    private const string BGM_VOL = "BGM";
    private const string SFX_VOL = "SFX";

    private void Start()
    {
        Init();
    }
    void Init()
    {
        UserSettings.ApplySettings();
    }
    public float MasterVolume { get { return Normalize(MASTER_VOL); } }
    public float BGMVolume { get { return Normalize(BGM_VOL); } }
    public float SFXVolume { get { return Normalize(SFX_VOL); } }
  
    private float Normalize(string paramName)
    {
        if (_mixer.GetFloat(paramName, out float dB))
        {
            return Mathf.Pow(10f, dB / 20f);
        }
        return 1f;
    }
    public void SetMasterVolume(float value) => SetVolume(MASTER_VOL, value);
    public void SetBGMVolume(float value) => SetVolume(BGM_VOL, value);
    public void SetSFXVolume(float value) => SetVolume(SFX_VOL, value);

    private void SetVolume(string paramName, float value)
    {
       
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
     
        _mixer.SetFloat(paramName, dB);
        

    }





}
