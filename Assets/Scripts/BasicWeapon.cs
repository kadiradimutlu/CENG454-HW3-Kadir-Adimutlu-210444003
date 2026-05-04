using UnityEngine;

public class BasicWeapon : IWeapon 
{
    public void Fire(Transform spawnPoint)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        
        if (bullet != null)
        {
            bullet.transform.position = spawnPoint.position;
            bullet.transform.rotation = spawnPoint.rotation;
            bullet.SetActive(true);
        }
    }
}