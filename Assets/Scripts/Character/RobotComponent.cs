using UnityEngine;
using Battle;
using System.Collections.Generic;

namespace Character
{
    public class RobotComponent : PropBaseComponent, IMovable, IAttackable, IHP, IWakeable
    {

        [SerializeField] Statistics hp = new(1);
        [SerializeField] List<SkillComponent> skills = new();

        readonly IMove walk = new Walk();
        public Statistics HP => hp;

        public bool IsWake { get; private set; }
        public float WakeDuration { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        #region Movement
        void IMovable.Move(Vector3 direction, float strength)
        {
            if (direction.sqrMagnitude < 0.01f) return;

            direction.Normalize();
            animator?.SetBool("IsMove", true);

            if (walk is IStrength s) s.SetStrength(strength);
            if (walk is IDirection d) d.SetDirection(direction);

            OnMove(direction, strength);
        }

        protected virtual void OnMove(Vector3 direction, float strength) { }

        public virtual void StopMove()
        {
            animator?.SetBool("IsMove", false);
        }
        #endregion

        #region Combat
        public virtual void Attack(int attackType = 0)
        {
            animator?.SetTrigger("Attack");
            animator?.SetInteger("AttackType", attackType);
            OnAttack(attackType);
        }

        protected virtual void OnAttack(int attackType)
        {
            if (skills == null || skills.Count == 0) return;

            int index = Mathf.Clamp(attackType, 0, skills.Count - 1);

            var skill = skills[index];

            if (skill == null) return;

            skill.SetCaster(transform);
            skill.Fire();
        }

        protected override void OnTakeDamage()
        {
            hp.Value--;
            if (hp.Value <= 0 && !IsDead)
                (this as IDeathable)?.Die();
        }

        public virtual void Wake()
        {
            IsWake = true;
            OnWake();
        }
        protected virtual void OnWake() { }
        
        #endregion
    }
}
