using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoreDamageFeedback : MonoBehaviour
{
    [Header("Feedback UI")]
    public TextMeshProUGUI feedbackText;
    public Image flashOverlay;

    [Header("Feedback Settings")]
    public string feedbackMessage = "CORE HIT! SURVIVAL TIME +5s";
    public float visibleDuration = 1.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.25f);

    private Coroutine feedbackRoutine;

    private void Awake()
    {
        HideFeedback();
    }

    private void OnEnable()
    {
        EnergyCore.OnCoreDamaged += HandleCoreDamaged;
    }

    private void OnDisable()
    {
        EnergyCore.OnCoreDamaged -= HandleCoreDamaged;
    }

    private void HandleCoreDamaged(int damageAmount)
    {
        if (feedbackRoutine != null)
        {
            StopCoroutine(feedbackRoutine);
        }

        feedbackRoutine = StartCoroutine(PlayFeedback());
    }

    private IEnumerator PlayFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = feedbackMessage;
            feedbackText.gameObject.SetActive(true);
        }

        if (flashOverlay != null)
        {
            flashOverlay.raycastTarget = false;
            flashOverlay.color = flashColor;
        }

        yield return new WaitForSecondsRealtime(visibleDuration);

        HideFeedback();
        feedbackRoutine = null;
    }

    private void HideFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(false);
        }

        if (flashOverlay != null)
        {
            Color hiddenColor = flashOverlay.color;
            hiddenColor.a = 0f;
            flashOverlay.color = hiddenColor;
            flashOverlay.raycastTarget = false;
        }
    }
}
