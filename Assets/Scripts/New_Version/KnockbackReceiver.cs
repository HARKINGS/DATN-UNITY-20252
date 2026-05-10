using UnityEngine;
using System.Collections;

public class KnockbackReceiver : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        StartCoroutine(KnockbackRoutine(direction, force, duration));
    }

    private IEnumerator KnockbackRoutine(
        Vector2 direction,
        float force,
        float duration)
    {
        rb.linearVelocity = direction * force;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
    }
}