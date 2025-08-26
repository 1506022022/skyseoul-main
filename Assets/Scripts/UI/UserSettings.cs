using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine.Rendering.Universal;

namespace Games.System.Settings
{
    public static class UserSettings
    {
        static private PlayerSettingData _settings;
        static public PlayerSettingData Settings
        {
            get
            {
                if (_settings == null) Init();
                return _settings;
            }
            private set { _settings = value; }
        }

        static private string SavePath => Path.Combine(Application.persistentDataPath, "PlayerSettings.json");

        static private readonly (int w, int h)[] Resolutions =
         {
            (1920, 1080),
            (1600, 900),
            (1280, 720)
         };

        static public void Init()
        {
            LoadSettings();
            ApplySettings();
        }
        static public void ApplySettings()
        {
            ApplyMouseKeyboard(Settings.MouseKeyBoard);
            ApplySound(Settings.Sound);
            ApplyGraphic(Settings.Graphic);
        }


        static public void UploadSettings(PlayerSettingData data)
        {
            Settings = data;
        }
        static public void ResetSettings()
        {
            Settings = new();
            SaveSettings();
        }
        #region Save&Load
        static public void SaveSettings()
        {
            string json = JsonUtility.ToJson(Settings, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Settings saved to {SavePath}");
        }
        static public void LoadSettings()
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);
                Settings = JsonUtility.FromJson<PlayerSettingData>(json);
            }
            else
                ResetSettings();

        }
        #endregion

        #region Mouse/KeyboardSetting
        static private void ApplyMouseKeyboard(MouseKeyBoardData data) { Settings.MouseKeyBoard = data; }
        #endregion

        #region SoundSetting
        static private void ApplySound(AudioData data) { AudioManager.ApplySettings(data); }

        #endregion

        #region GraphicSetting
        static private void ApplyGraphic(GraphicSettingData data)
        {
            SetDisplayMode(data.DisplayModeIndex);
            SetResolution(data.ResolutionIndex);
            SetGraphicQuality(data.PerformanceIndex);
        }
        static private void SetDisplayMode(int index)
        {
            switch (index)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
        static private void SetResolution(int index)
        {
            if (index < 0 || index >= Resolutions.Length)
                return;

            int w = Resolutions[index].w;
            int h = Resolutions[index].h;

            Screen.SetResolution(w, h, Screen.fullScreenMode);

      
        }

        static public void SetGraphicQuality(int index)
        {

            var pipelineAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;

            if (pipelineAsset != null)
            {
                switch (index)
                {
                    case 0:
                        pipelineAsset.shadowDistance = 10f;
                        pipelineAsset.msaaSampleCount = 0;
                        pipelineAsset.renderScale = 0.7f;
                        pipelineAsset.shadowCascadeCount = 1;
                        pipelineAsset.supportsHDR = false;
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                        QualitySettings.lodBias = 0.6f;
                        break;

                    case 1:
                        pipelineAsset.shadowDistance = 30f;
                        pipelineAsset.msaaSampleCount = 2;
                        pipelineAsset.renderScale = 0.85f;
                        pipelineAsset.shadowCascadeCount = 2;
                        pipelineAsset.supportsHDR = true;
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        QualitySettings.lodBias = 1.0f;
                        break;

                    case 2:
                        pipelineAsset.shadowDistance = 70f;
                        pipelineAsset.msaaSampleCount = 4;
                        pipelineAsset.renderScale = 1.0f;
                        pipelineAsset.shadowCascadeCount = 4;
                        pipelineAsset.supportsHDR = true;
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        QualitySettings.lodBias = 1.5f;
                        break;
                }
            }

        }

        #endregion

    }

}



