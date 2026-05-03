using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    
    private IMovementStrategy currentStrategy;

    void Start()
    {
        EnergyCore core = Object.FindFirstObjectByType<EnergyCore>();
        if (core != null)
        {
            target = core.transform;
        }

        SetStrategy(new DirectMovement());
    }

    void Update()
    {
        if (currentStrategy != null && target != null)
        {
            currentStrategy.Move(transform, target, speed);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetStrategy(new ZigzagMovement());
            Debug.Log("Hareket Stratejisi Değişti: ZIGZAG");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetStrategy(new DirectMovement());
            Debug.Log("Hareket Stratejisi Değişti: DIRECT (DOĞRUDAN)");
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
            
            Destroy(gameObject); 
        }
    }
}