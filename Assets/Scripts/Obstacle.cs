using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    // Pürüzsüz dönüş hızı
    public float rotationSpeed = 10f; 
    private Transform target;
    private IMovementStrategy currentStrategy;
    private NavMeshAgent agent;

    public GameObject explosionPrefab;
    public int scoreValue = 10;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (agent != null)
        {
            agent.updateRotation = false;
        }

        EnergyCore core = Object.FindFirstObjectByType<EnergyCore>();
        if (core != null)
        {
            target = core.transform;
        }
    }

    void Update()
    {
        if (currentStrategy != null && target != null && agent != null && agent.isOnNavMesh)
        {
            currentStrategy.Move(agent, target, speed);
            
            Vector3 lookDirection = (target.position - transform.position).normalized;
            lookDirection.y = 0; 

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
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