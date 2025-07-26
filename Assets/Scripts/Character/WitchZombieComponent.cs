namespace Character
{
    public class WitchZombieComponent : EnemyComponent, IAlert
    {
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        void IAlert.Alert()
        {
            animator.SetBool("Alert",true);
            OnAlert();
        }
        protected virtual void OnAlert() { }
        void IAlert.Release()
        {
            animator.SetBool("Alert", false);
            OnReleaseAlert();
        }
        protected virtual void OnReleaseAlert() { }
    }
}