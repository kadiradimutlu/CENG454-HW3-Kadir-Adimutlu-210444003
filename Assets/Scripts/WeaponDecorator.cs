using UnityEngine;

public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon wrappedWeapon;

    public WeaponDecorator(IWeapon weaponToWrap)
    {
        wrappedWeapon = weaponToWrap;
    }

    public virtual void Fire(Transform spawnPoint, Transform playerCamera)
    {
        wrappedWeapon.Fire(spawnPoint, playerCamera);
    }
}