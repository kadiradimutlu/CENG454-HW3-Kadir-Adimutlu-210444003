using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    
    [Header("Pool Settings")]
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
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

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab, transform);
            obj.SetActive(false); 
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            pool.Enqueue(obj);
            return obj;
        }
        return null;
    }
}