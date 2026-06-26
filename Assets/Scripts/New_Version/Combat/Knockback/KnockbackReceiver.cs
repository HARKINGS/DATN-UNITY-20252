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
        Debug.Log("Apply Knockback!");
        StartCoroutine(KnockbackRoutine(knockbackEntityTransform, knockbackForce, knockbackTime, stunTime));
    }

    private IEnumerator KnockbackRoutine(
        Transform entity,
        float knockbackForce,
        float knockbackTime,
        float stunTime)
    {
        var statusMachine = GetComponent<CharacterStatusMachine>();
        var movement = GetComponent<CharacterMovement>();

        // 1. Bắt đầu đẩy lùi -> Trạng thái Hurt
        Vector2 direction = (transform.position - entity.position).normalized;
        rb.linearVelocity = direction * knockbackForce;

        statusMachine.ChangeStatus(CharacterStatus.Hurt);

        // KHÔNG TẮT CharacterMovement - để BossBrain vẫn chạy logic
        // Chỉ set velocity qua Rigidbody

        yield return new WaitForSeconds(knockbackTime);

        // 2. Hết đẩy lùi, dừng lực
        rb.linearVelocity = Vector2.zero;

        // Nếu stun time > 0 → Stun, ngược lại về Idle luôn
        if (stunTime > 0)
        {
            statusMachine.ChangeStatus(CharacterStatus.Stun);
            yield return new WaitForSeconds(stunTime);
        }

        // 3. Trả tự do cho Boss về Idle
        statusMachine.ChangeStatus(CharacterStatus.Idle);
    }
}