using System.Collections.Generic;
using Unity.AppUI.UI;
using UnityEngine;

namespace GameUI
{
    public class Notification : UIWidget
    {
        [Header("알림이 배치될 부모")]
        [SerializeField] RectTransform content;
        [Header("알림 메시지 Data")]
        [SerializeField] NotificationDatabase notifyData;
        [SerializeField] GameObject textObject;
        [Range(0, 10)]
        [SerializeField] float disappearTime = 3f;
        [Header("알림 간격 (Y 오프셋)")]
        [SerializeField] float spacing = 50f;

        Queue<NotifyText> notifyTexts = new();

        private void OnDisable() => ClearAllSlot();

        public void ShowMessage(string type, string target = "")
        {
            string message = string.IsNullOrEmpty(target)
                ? notifyData.GetMessage(type)
                : notifyData.GetMessage(type, target);

            if (!string.IsNullOrEmpty(message))
                CreateMessage(message);
        }

        private void CreateMessage(string message)
        {
            GameObject obj = Instantiate(textObject, content);
            RectTransform rect = obj.GetComponent<RectTransform>();

          
            int index = notifyTexts.Count;
            rect.anchoredPosition = new Vector2(0, -index * spacing);

            NotifyText text = obj.GetComponent<NotifyText>();
            if (text == null) return;

            text.OnExpired += HandleExpired;
            text.SetMessage(message, disappearTime);

            notifyTexts.Enqueue(text);
        }

        private void HandleExpired(NotifyText expiredText)
        {
            if (notifyTexts.Count > 0 && notifyTexts.Peek() == expiredText)
                notifyTexts.Dequeue();

            Destroy(expiredText.gameObject);

          
            RepositionNotifications();
        }

        private void RepositionNotifications()
        {
            int i = 0;
            foreach (var notify in notifyTexts)
            {
                if (notify != null)
                {
                    var rect = notify.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(0, -i * spacing);
                    i++;
                }
            }
        }

        public void ClearAllSlot()
        {
            while (notifyTexts.Count > 0)
            {
                var text = notifyTexts.Dequeue();
                if (text != null)
                    Destroy(text.gameObject);
            }
        }
    }
}
