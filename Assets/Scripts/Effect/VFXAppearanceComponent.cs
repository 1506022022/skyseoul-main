using UnityEngine;
using System;
using INab.Common;

namespace Effect
{
    public interface IAppearance
    {
        float Duration { get; }
        event Action OnDissolve;
        event Action OnAppear;
        void InvokeDissolve();
        void InvokeAppear();
    }
   
    public class VFXAppearanceComponent : MonoBehaviour, IAppearance
    {
        public event Action OnDissolve;
        public event Action OnAppear;

        [Header("Duration")]
        [SerializeField] float duration;

        [Header("VFX")]
        [SerializeField] InteractiveEffect dissolve;
        [SerializeField] InteractiveEffect appear;

        public float Duration => duration;


        void OnEnable()
        {
            OnAppear += AppearEffect;
            OnDissolve += DissolveEffect;
        }

        void OnDisable()
        {
            OnAppear -= AppearEffect;
            OnDissolve -= DissolveEffect;
        }

        void AppearEffect() => appear?.PlayEffect();
        void DissolveEffect()=> dissolve?.PlayEffect();
        public void InvokeDissolve() => OnDissolve?.Invoke();
        public void InvokeAppear() => OnAppear?.Invoke();
#if UNITY_EDITOR
        public void SyncDuration()
        {
            if (dissolve != null) dissolve.duration = duration;
            if (appear != null) appear.duration = duration;
        }
#endif
    }
}
