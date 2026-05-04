using UnityEngine;

public class DoubleShotWeapon : WeaponDecorator
{
    public DoubleShotWeapon(IWeapon weaponToWrap) : base(weaponToWrap) { }

    public override void Fire(Transform spawnPoint)
    {
        base.Fire(spawnPoint);

        GameObject extraBullet = BulletPool.Instance.GetBullet();
        if (extraBullet != null)
        {
            extraBullet.transform.position = spawnPoint.position;
            extraBullet.transform.rotation = spawnPoint.rotation * Quaternion.Euler(0, 15f, 0);
            extraBullet.SetActive(true);
        }
    }
}