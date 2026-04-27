using UnityEngine;

public class Obstacle : MonoBehaviour, IDamageable
{
    public int health = 20;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("Obstacle destroyed!");
        }
    }
}