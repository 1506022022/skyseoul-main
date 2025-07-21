using Unity.Behavior;
using UnityEngine;

public class ThrowableComponent : WeaponBaseComponent
{
    [SerializeField] AttackBoxComponent container;
    [SerializeField] float threshold;
    Rigidbody rigid;
    bool throwed;
    Transform parent;
    Vector3 localPosition;
    Quaternion localRotation;

    public void Throw(Vector3 dir, Vector3 power)
    {
        if (!container || !rigid) return;
        Reset();
        throwed = true;
        transform.SetParent(null);
        rigid.isKinematic = false;
        rigid.linearVelocity = dir;
        rigid.AddForce(dir * power.magnitude, ForceMode.Impulse);
    }

    protected override void Initialize(Transform owner)
    {
        if (container == null) return;
        container.SetActor(owner);
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        parent = transform.parent;
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
    }
    private void Update()
    {
        Util.Enumerator.InvokeFor(transform.GetComponentsInChildren<BehaviorGraphAgent>(), x => x.enabled = false);

        if (!throwed) return;
        if (container.AttackBox.NotWithinAttackWindow) container.OpenAttackWindow();
        throwed = threshold < rigid.linearVelocity.magnitude;
        if (!throwed)
        {
            Util.Enumerator.InvokeFor(transform.GetComponentsInChildren<BehaviorGraphAgent>(), x => x.enabled = true);
            transform.DetachChildren();
            Reset();
        }
    }
    private void Reset()
    {
        rigid.linearVelocity = Vector3.zero;
        rigid.isKinematic = true;

        transform.SetParent(parent);
        transform.SetLocalPositionAndRotation(localPosition, localRotation);
    }
}