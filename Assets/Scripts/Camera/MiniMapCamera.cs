using GameUI;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCamera
{
    public class MiniMapCamera : MonoBehaviour,IInitializable
    {
        [Header("위치 오프셋")]
        [SerializeField] Vector3 offset;
        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            transform.position = transform.parent.position + offset;

            BattleHUD hud = (BattleHUD)UIController.Instance.MainHUD;
            hud.SettingMiniMapTarget(transform.parent);

        }

       
     
    }
}
