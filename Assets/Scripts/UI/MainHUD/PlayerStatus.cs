using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    [Serializable]
    public class StatusBar : IStatusBar
    {
        [Header("Status 슬라이더")]
        [SerializeField] Slider fillArea;
   
        public void UpdateStatusBar(float value)
        {
            if (fillArea == null) return;
            value = Mathf.Clamp01(value);
            fillArea.value = value;

        }
    }

    public class PlayerStatus:UIWidget
    {
        [Header("Status Bars")]
        [SerializeField] private StatusBar playerHp;
        [SerializeField] private StatusBar playerImpairment;
        
        public void UpdatePlayerHp(float value) => playerHp.UpdateStatusBar(value);
        public void UpdateImpairment(float value) => playerImpairment.UpdateStatusBar(value);
    }
}