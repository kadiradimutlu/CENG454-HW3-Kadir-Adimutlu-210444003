using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject bulletPrefab;
    public int poolSize = 20;
    public bool allowExpansion = true;

    private readonly List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateBullet();
        }
    }

    private GameObject CreateBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("BulletPool is missing a bullet prefab reference.");
            return null;
        }

        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        pool.Add(bullet);
        return bullet;
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i] != null && !pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        if (allowExpansion)
        {
            return CreateBullet();
        }

        Debug.LogWarning("BulletPool has no inactive bullets available.");
        return null;
    }
}
