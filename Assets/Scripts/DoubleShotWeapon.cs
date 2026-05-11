using UnityEngine;

public class DoubleShotWeapon : WeaponDecorator
{
    public DoubleShotWeapon(IWeapon weaponToWrap) : base(weaponToWrap) { }

    public override void Fire(Transform spawnPoint, Transform playerCamera)
    {
        base.Fire(spawnPoint, playerCamera);

        GameObject extraBullet = BulletPool.Instance.GetBullet();
        if (extraBullet != null)
        {
            Vector3 targetPoint;
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(100f);

            Vector3 shootDirection = (targetPoint - spawnPoint.position).normalized;

            extraBullet.transform.position = spawnPoint.position;
            extraBullet.transform.rotation = Quaternion.LookRotation(shootDirection) * Quaternion.Euler(0, 5f, 0);
            extraBullet.SetActive(true);
        }
    }
}