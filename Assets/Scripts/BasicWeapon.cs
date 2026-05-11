using UnityEngine;

public class BasicWeapon : IWeapon 
{
    public void Fire(Transform spawnPoint, Transform playerCamera)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        
        if (bullet != null)
        {
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
            bullet.transform.rotation = Quaternion.LookRotation(shootDirection);
            bullet.SetActive(true);
        }
    }
}