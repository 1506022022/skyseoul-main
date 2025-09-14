using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class EnemyComponent : CharacterBaseComponent, IEnemy, ITraveler, IStun, IHitStun, IGrab, IThrow,IStatusable
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
            (this as IHitStun)?.HitStun(3f);
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
        void IGrab.Grab(Transform target)
        {
            if (TryGetComponent<GrabInfo>(out var grabInfo))
            {
                StartCoroutine(LateGrab(grabInfo, target));
            }
            animator.SetBool("Grab", true);
            OnGrab(target);
        }
        IEnumerator LateGrab(GrabInfo grabInfo, Transform grabTarget)
        {
            yield return new WaitForSeconds(grabInfo.GrabTransformDelay);
            if (grabTarget == null || grabInfo.GrabTransform == null) yield break;
            grabTarget.SetParent(grabInfo.GrabTransform);
            grabTarget.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        protected virtual void OnGrab(Transform target) { }

        void IGrab.Drop()
        {
            animator.SetBool("Grab", false);
            OnDrop();
        }
        protected virtual void OnDrop() { }

        void IThrow.Throw(Vector3 dir, Vector3 power)
        {
            animator.SetTrigger("Attack");
            animator.SetInteger("AttackType", 2);
            OnThrow(dir, power);
        }
        protected virtual void OnThrow(Vector3 dir, Vector3 power) { }
    }
}