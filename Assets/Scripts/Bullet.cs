using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public int damage = 10;
    public float lifeTime = 2f;

    private float timer;
    private TrailRenderer[] trailRenderers;

    private void Awake()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        timer = 0f;

        if (trailRenderers != null)
        {
            foreach (TrailRenderer trail in trailRenderers)
            {
                if (trail != null)
                {
                    trail.Clear();
                }
            }
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }
}
