using UnityEngine;


    [System.Serializable]
    public class PlayerSettingData
    {
        public MouseKeyBoardData MouseKeyBoard = new();
        public AudioData Sound = new();
        public GraphicSettingData Graphic = new();
    }
    [System.Serializable]
    public class MouseKeyBoardData
    {
        public int MouseSensivity = 100;
    }
    [System.Serializable]
    public class AudioData
    {
        public int TotalVolume = 100;
        public int BGMVolume = 100;
        public int EffectVolume = 100;
        public bool RunInBackGround = true;
    }
    [System.Serializable]
    public class GraphicSettingData
    {
        public int DisplayModeIndex = 0;
        public int ResolutionIndex = 0;
        public int PerformanceIndex = 2;
    }
