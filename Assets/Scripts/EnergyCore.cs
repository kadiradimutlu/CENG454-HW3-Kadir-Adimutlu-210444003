using System;
using UnityEngine;

public class EnergyCore : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    public static event Action<int, int> OnCoreHealthChanged;
    public static event Action OnCoreDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
        OnCoreHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        
        OnCoreHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnCoreDestroyed?.Invoke();
            Debug.Log("Core Destroyed! Game Over.");
        }
    }
}