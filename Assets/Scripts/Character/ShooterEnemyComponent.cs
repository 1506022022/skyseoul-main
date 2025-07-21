using Character;
using System.Collections;
using UnityEngine;

public class ShooterEnemyComponent : EnemyComponent
{
    [SerializeField] ThrowableComponent throwable;
    protected override void OnThrow(Vector3 dir, Vector3 power)
    {
        base.OnThrow(dir, power);
        if (throwable == null) return;
        StartCoroutine(LateThrow(dir, power));
    }
    IEnumerator LateThrow(Vector3 dir, Vector3 power)
    {
        var delay = 0f;
        if (TryGetComponent<ThrowInfo>(out var throwInfo)) { delay = throwInfo.ThrowDelay; }
        yield return new WaitForSeconds(delay);
        throwable.transform.SetParent(null);
        throwable.Throw(dir, power);
    }
}