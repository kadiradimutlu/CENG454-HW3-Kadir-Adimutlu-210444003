using UnityEngine;

public class EnergyCore : MonoBehaviour, IDamageable
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Core took damage! Current health: " + health);
    }
}