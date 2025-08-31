using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentCell : MonoBehaviour
{
    [SerializeField] Image slotImage;
    [SerializeField] Image weaponImage;
    [SerializeField] TextMeshProUGUI bulletCount;

    public void UpdateSlot(Sprite sprite, Vector2 size) 
    { 
        slotImage.sprite = sprite;
        RectTransform rt = slotImage.GetComponent<RectTransform>();
        if (rt != null)
            rt.sizeDelta = size;
    }
    public void UpdateWeaponImage(Sprite sprite) { weaponImage.sprite = sprite; }

    public void UpdateBulletCount(int bullet) { bulletCount.text = bullet + "/¡Ä"; }
}
