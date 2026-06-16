using UnityEngine;
using System.Collections;
using System;

public class KnockbackReceiver : MonoBehaviour, IKnockbackable
{
    private Rigidbody2D rb;
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Transform knockbackEntityTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        GetComponent<CharacterAnimation>().InterruptCurrentSkill();
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Hurt);
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(KnockbackRoutine(knockbackEntityTransform, knockbackForce, knockbackTime, stunTime));
    }

    private IEnumerator KnockbackRoutine(
        Transform entity,
        float knockbackForce,
        float knockbackTime,
        float stunTime)
    {
        var statusMachine = GetComponent<CharacterStatusMachine>();
        var animator = GetComponent<CharacterAnimation>();

        // 1. Bắt đầu đẩy lùi -> Trạng thái Hurt
        Vector2 direction = (transform.position - entity.position).normalized;
        rb.linearVelocity = direction * knockbackForce;

        statusMachine.ChangeStatus(CharacterStatus.Hurt);

        yield return new WaitForSeconds(knockbackTime);

        // 2. Hết đẩy lùi, dừng lực -> Chuyển sang Choáng (Stun)
        rb.linearVelocity = Vector2.zero;
        statusMachine.ChangeStatus(CharacterStatus.Stun);

        yield return new WaitForSeconds(stunTime);

        // 3. QUAN TRỌNG: Hết thời gian choáng, trả tự do cho Boss về Idle
        statusMachine.ChangeStatus(CharacterStatus.Idle);
    }
}