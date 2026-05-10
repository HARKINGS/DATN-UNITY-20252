using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealthWidth;

    public TMP_Text healthText;
    public Animator healthTextAnim;
    public RectTransform hpRect;

    private void Start()
    {
        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        healthText.text = StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        maxHealthWidth = hpRect.sizeDelta.x;
    }

    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        healthTextAnim.Play("TextUpdate");

        healthText.text = StatsManager.Instance.currentHealth + "/" + StatsManager.Instance.maxHealth;
        UpdateHealthBar();
        
        if (StatsManager.Instance.currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
            return;
        }

        StartCoroutine(HurtEffectRoutine(0.25f));
    }

    public void UpdateHealthBar()
    {
        float hpPercent = 1.0f * StatsManager.Instance.currentHealth / StatsManager.Instance.maxHealth;
        float newWidth = maxHealthWidth * hpPercent;
        hpRect.sizeDelta = new Vector2(newWidth, hpRect.sizeDelta.y);
    }

    IEnumerator HurtEffectRoutine(float hurtTime)
    {
        GetComponent<Animator>().SetBool("isHurt", true);
        yield return new WaitForSeconds(hurtTime);
        GetComponent<Animator>().SetBool("isHurt", false);
    }

    IEnumerator DeathRoutine()
    {
        GetComponent<Animator>().SetBool("isDead", true);

        // Vô hiệu hóa các script điều khiển và va chạm để tránh lỗi logic khi đang chết
        GetComponent<PlayerMovement>().enabled = false;
        //GetComponent<Collider2D>().enabled = false;
        //GetComponent<Rigidbody2D>().simulated = false;

        // Đợi thời gian animation chết chạy (ví dụ 2 giây)
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        // Hoặc Destroy(gameObject);
    }
}
