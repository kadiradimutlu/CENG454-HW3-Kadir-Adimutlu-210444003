using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    private IMovementStrategy currentStrategy;

    public GameObject explosionPrefab;
    public int scoreValue = 10;

    void Start()
    {
        EnergyCore core = Object.FindFirstObjectByType<EnergyCore>();
        if (core != null)
        {
            target = core.transform;
        }
    }

    void Update()
    {
        if (currentStrategy != null && target != null)
        {
            currentStrategy.Move(transform, target, speed);
        }
    }

    public void SetStrategy(IMovementStrategy newStrategy)
    {
        currentStrategy = newStrategy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyCore"))
        {
            EnergyCore core = other.GetComponent<EnergyCore>();
            if (core != null)
            {
                core.TakeDamage(10); 
            }
            Destroy(gameObject); 
        }
        else if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false); 
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); 
        }
    }
}