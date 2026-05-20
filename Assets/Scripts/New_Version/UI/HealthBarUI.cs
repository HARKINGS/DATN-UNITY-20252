using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health;

    private float maxHealthWidth;
    public TMP_Text healthText;
    public Animator healthTextAnim;
    public RectTransform hpRect;

    private void Awake()
    {
        if(health == null) 
            health = GetComponent<CharacterHealth>();
        maxHealthWidth = hpRect.sizeDelta.x;
        UpdateUI(health.GetCurrentHealth());
    }

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
        //Debug.Log("current HP is: " + current);
        healthText.text = current + "/" + health.stats.MaxHealth;
        float hpPercent = 1.0f * current / health.stats.MaxHealth;
        float newWidth = maxHealthWidth * hpPercent;
        hpRect.sizeDelta = new Vector2(newWidth, hpRect.sizeDelta.y);
    }
}