using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class QuickSlotCell : MonoBehaviour
    {
       
        [Header("Slot")]
        [SerializeField] Image slotImage;

        [Header("Item")]
        [SerializeField] TextMeshProUGUI itemCount;
        [SerializeField] Image itemImage;

        public void  ChangeSlotInfo(Sprite sprite, Vector2 size)
        {
            slotImage.sprite = sprite;
            RectTransform rt = slotImage.GetComponent<RectTransform>();
            if (rt != null)
                rt.sizeDelta = size;
        }

        public void AddItem(Sprite sprite, int count)
        {
            itemImage.sprite = sprite;
            ChangeItemCount(count);
        }
        public void ChangeItemCount(int amount)
        {
            itemCount.text = amount.ToString(); 
        }
            
     }
}