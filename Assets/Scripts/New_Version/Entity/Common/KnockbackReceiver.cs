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
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Hurt);
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
        // 1. Bắt đầu đẩy lùi -> Trạng thái Hurt
        Vector2 direction = (transform.position - entity.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Hurt);

        yield return new WaitForSeconds(knockbackTime);

        // 2. Hết đẩy lùi, dừng lực -> Chuyển sang Choáng (Stun)
        rb.linearVelocity = Vector2.zero;
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Stun);

        yield return new WaitForSeconds(stunTime);

        // 3. QUAN TRỌNG: Hết thời gian choáng, trả tự do cho Boss về Idle
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Idle);
    }
}