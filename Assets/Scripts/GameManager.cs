using UnityEngine;

public class GameManager : MonoBehaviour
{
    void OnEnable()
    {
        EnergyCore.OnCoreDestroyed += HandleGameOver;
    }

    void OnDisable()
    {
        EnergyCore.OnCoreDestroyed -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        Debug.Log("GAME OVER! The core has been breached.");
    }
}