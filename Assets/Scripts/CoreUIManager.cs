using UnityEngine;
using TMPro; 

public class CoreUIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    void OnEnable()
    {
        EnergyCore.OnCoreHealthChanged += UpdateHealthUI;
    }

    void OnDisable()
    {
        EnergyCore.OnCoreHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = "CORE HP: " + currentHealth + " / " + maxHealth;
            Debug.Log("UI Updated: " + currentHealth);
        }
    }
}