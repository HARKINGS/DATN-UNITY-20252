using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            StartCoroutine(DeathRoutine());
            return;
        }

        if (amount < 0)
        {
            StartCoroutine(HurtEffectRoutine(0.5f));
        }
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
        GetComponent<EnemyMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        // Đợi thời gian animation chết chạy (ví dụ 2 giây)
        yield return new WaitForSeconds(2f);

        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
