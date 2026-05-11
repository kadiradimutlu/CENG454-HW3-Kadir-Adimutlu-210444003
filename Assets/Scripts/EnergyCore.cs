using System;
using UnityEngine;

public class EnergyCore : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth;

    public static event Action<int, int> OnCoreHealthChanged;
    public static event Action<int> OnCoreDamaged;
    public static event Action OnCoreDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
        OnCoreHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        int previousHealth = currentHealth;

        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        int actualDamage = previousHealth - currentHealth;

        OnCoreHealthChanged?.Invoke(currentHealth, maxHealth);

        if (actualDamage > 0)
        {
            OnCoreDamaged?.Invoke(actualDamage);
        }

        if (currentHealth == 0)
        {
            OnCoreDestroyed?.Invoke();
        }
    }
}
