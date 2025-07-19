using Battle;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class EnemyComponent : CharacterBaseComponent, IEnemy, ITraveler, IStun, IHitStun
    {
        [SerializeField] List<SkillComponent> attacks = new();

        float stunTime;
        bool IStun.IsStun => Time.time < stunTime;
        void IStun.Stun(float stunDuration) => stunTime = Time.time;
        float hitTime;
        bool IHitStun.IsHit => Time.time < hitTime;
        void IHitStun.HitStun(float hitDuration) => hitTime = Time.time + hitDuration;

        protected override void OnTakeDamage()
        {
            (this as IHitStun)?.HitStun(0.5f);
        }

        void ITraveler.StartTravel()
        {
            animator.SetBool("IsMove", true);
            animator.SetFloat("MoveSpeed", 1.0f);
            OnStartTravel();
        }
        protected virtual void OnStartTravel() { }

        void ITraveler.EndTravel()
        {
            animator.SetBool("IsMove", false);
            animator.SetFloat("MoveSpeed", 0f);
            OnEndTravel();
        }
        protected virtual void OnEndTravel() { }
        protected override void OnAttack(int attackType)
        {
            base.OnAttack(attackType);
            attackType -= 1;
            if (attacks.Count > attackType && attacks[attackType] != null) attacks[attackType].Fire();
        }
    }
}