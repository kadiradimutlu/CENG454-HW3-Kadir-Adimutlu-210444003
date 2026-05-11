using UnityEngine;

public class TripleShotWeapon : WeaponDecorator
{
    private readonly float spreadAngle;

    public TripleShotWeapon(IWeapon weaponToWrap, float spreadAngle = 6f) : base(weaponToWrap)
    {
        this.spreadAngle = spreadAngle;
    }

    public override void Fire(Transform spawnPoint, Transform playerCamera)
    {
        // Center bullet from the wrapped basic weapon
        base.Fire(spawnPoint, playerCamera);

        // Two extra bullets for the ultimate spread
        FireExtraBullet(spawnPoint, playerCamera, -spreadAngle);
        FireExtraBullet(spawnPoint, playerCamera, spreadAngle);
    }

    private void FireExtraBullet(Transform spawnPoint, Transform playerCamera, float yawOffset)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        if (bullet == null) return;

        Vector3 targetPoint;
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 shootDirection = (targetPoint - spawnPoint.position).normalized;

        bullet.transform.position = spawnPoint.position;
        bullet.transform.rotation =
            Quaternion.LookRotation(shootDirection) * Quaternion.Euler(0f, yawOffset, 0f);

        bullet.SetActive(true);
    }
}
