using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public static class AudioManager 
{
    private static AudioPlayer audioRoot = null;

 
    public static void Init()
    {
        if (audioRoot == null)
        { 
            audioRoot = Object.FindAnyObjectByType<AudioPlayer>();
            GameObject obj;
            if (audioRoot == null)
            {
                GameObject audio = Resources.Load<GameObject>("AudioPlayer");
                obj = Object.Instantiate(audio);
                audioRoot = obj.GetComponent<AudioPlayer>();
            }
            Object.DontDestroyOnLoad(audioRoot);
          
        }
    }
    static AudioManager()
    {
        Init();
    }
    public  static void ApplyMasterVolume(float volume)
      => audioRoot.SetMasterVolume(Mathf.Clamp01(volume / 100f));

    public static  void ApplyBGMVolume(float volume)
        => audioRoot.SetBGMVolume(Mathf.Clamp01(volume / 100f));

    public  static void ApplySFXVolume(float volume)
        => audioRoot.SetSFXVolume(Mathf.Clamp01(volume / 100f));

    public static void ApplySettings(AudioData settings)
    {
        ApplyMasterVolume(settings.TotalVolume);
        ApplyBGMVolume(settings.BGMVolume);
        ApplySFXVolume(settings.EffectVolume);
        Application.runInBackground = settings.RunInBackGround;
    }



}
