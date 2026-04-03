using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    private float maxHealthWidth;

    public TMP_Text healthText;
    public Animator healthTextAnim;
    public RectTransform hpRect;

    private void Start()
    {
        currentHealth = maxHealth;
        healthText.text = currentHealth + "/" + maxHealth;
        maxHealthWidth = hpRect.sizeDelta.x;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthTextAnim.Play("TextUpdate");
        healthText.text = currentHealth + "/" + maxHealth;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            // game object mà hàm được inject vào
            gameObject.SetActive(false);
        }
    }

    public void UpdateHealthBar()
    {
        float hpPercent = 1.0f * currentHealth / maxHealth;
        float newWidth = maxHealthWidth * hpPercent;
        hpRect.sizeDelta = new Vector2(newWidth, hpRect.sizeDelta.y);
    }
}
