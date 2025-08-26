using Character;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class BattleHUD : UIHUD
    {
        PlayerStatus statusBar;
        QuickSlot quickSlot;
        Equipment equipment;
        public override bool Init()
        {
            if (!base.Init()) return false;
            GetBattleWidgets();
            return true;
        }

        void GetBattleWidgets()
        {
            statusBar = GetWidget<PlayerStatus>();
            quickSlot = GetWidget<QuickSlot>();
            equipment = GetWidget<Equipment>();
        }

        public void UpdatePlayerHp(IHP health) { statusBar.UpdatePlayerHp(health.HP.Ratio); }
        public void UpdatePlayerStamina(float ratio) { statusBar.UpdateImpairment(ratio); }
       
    }
       
} 

