using UnityEngine;

public class GunComponent : WeaponBaseComponent
{
    Gun gun;

    protected override void Initialize(Transform owner)
    {
        gun = new(transform, owner);
    }
    public void Fire() => gun?.Fire();
}