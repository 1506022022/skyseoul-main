using UnityEngine;

namespace GameUI
{
    public class MiniMap :UIWidget
    {
        [Header("방향 공전")]
        [SerializeField] RectTransform rotateTransform;

        [Header("공전 반대방향 자전 객체들")]
        [SerializeField] RectTransform[] directionLabels;
        Transform target;

        
        private void Update()
        {
            if (target != null)
                RotateWithTarget();
        }
        void RotateWithTarget()
        {
            float playerY = target.eulerAngles.y;
            rotateTransform.localEulerAngles = new Vector3(0, 0, -playerY);
            RotateLabels(playerY);
        }

        void RotateLabels(float playerY)
        {
            for(int i  =0; i< directionLabels.Length; i++) 
            {
                directionLabels[i].localEulerAngles = new Vector3(0, 0, playerY);
            }
        }
        public void SetTarget(Transform target) { this.target = target; }
    }
}