using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI :
    MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health;

    [SerializeField]
    private Image fill;

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateUI;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateUI;
    }

    private void UpdateUI(int current)
    {
        fill.fillAmount = (float) current;
    }
}