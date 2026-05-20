using UnityEngine;
using System.Collections;

public class KnockbackReceiver : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Transform knockbackEntityTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        //Debug.Log("Knockback Apply!");
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(KnockbackRoutine(knockbackEntityTransform, knockbackForce, knockbackTime, stunTime));
    }

    private IEnumerator KnockbackRoutine(
        Transform entity,
        float knockbackForce,
        float knockbackTime,
        float stunTime)
    {
        //Debug.Log("Knockback Data is: " + knockbackTime + " " + knockbackForce + " " + stunTime);

        Vector2 direction = (transform.position - entity.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        
    }
}