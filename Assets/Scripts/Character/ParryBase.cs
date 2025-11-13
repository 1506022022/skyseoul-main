using Battle;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/ParryActionSO")]
public class ParryActionSO : ScriptableObject
{
    [Header("Parry Settings")]
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float activeDuration = 0.3f;
    [SerializeField] private bool triggerHackOnSuccess = false;

    private float timer;

    public virtual void Execute(IActor defender)
    {
        timer = activeDuration;

       TryParry(defender);
    }

    
    protected virtual void TryParry(IActor defender)
    {
        if (defender is not Transform transform) return;

        Transform origin = transform;


        if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, rayDistance, targetMask))
        {
            var target = hit.collider.GetComponentInParent<IActor>();

            if (target != null && IsActorAttacking(target))
            {
                var ctx = new ParryContext
                {
                    Defender = defender,
                    Attacker = target,
                    HitPoint = hit.point
                };

                OnParrySuccess(ctx);
                return;
            }
        }

        OnParryFail(defender);
    }

   
    protected virtual bool IsActorAttacking(IActor actor)
    {
       
        return false;
    }


    
    protected virtual void OnParrySuccess(ParryContext ctx)
    {
        Debug.Log($"[SO] Parry success vs {ctx.Attacker}");

        if (triggerHackOnSuccess && ctx.Defender is MonoBehaviour mb)
        {
            
        }
    }

    /// <summary>
    /// 패리 실패 시 처리
    /// </summary>
    protected virtual void OnParryFail(IActor defender)
    {
        Debug.Log($"[SO] Parry failed by {defender}");
    }
}
