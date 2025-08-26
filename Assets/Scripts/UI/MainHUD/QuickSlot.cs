using System;
using UnityEngine;
using UnityEngine.UI;


namespace GameUI
{
    [System.Serializable]
    public class SlotSprite
    {
        [Header("Select")]
        public Sprite selectSprite;
        public Vector2 selectSize;

        [Header("UnSelect")]
        public Sprite unselectSprite;
        public Vector2 unselectSize;
    }
    public class QuickSlot : UIWidget,IButton,ISelectable<int>
    {
        [Header("Slots")]
        [SerializeField]QuickSlotCell[] slots;

        [SerializeField] SlotSprite sprite;

        public override bool Init()
        {
            if (base.Init() == false) return false;

            BindButtonEvents();
            return true;
        }

        public void BindButtonEvents()
        {

            for (int i = 0; i < slots.Length; i++)
            {
                int index = i;
                Button slotButton = slots[i].GetComponent<Button>();
                if (slotButton == null) continue;
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => Select(index));
            }
        }
        public void Select(int index)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (index == i) slots[i].ChangeSlotInfo(sprite.selectSprite, sprite.selectSize);
                else slots[i].ChangeSlotInfo(sprite.unselectSprite, sprite.unselectSize);
            }
        }

        
      
    }
}