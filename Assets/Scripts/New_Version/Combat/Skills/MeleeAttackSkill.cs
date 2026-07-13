using System.Collections;
using UnityEngine;

public class MeleeAttackSkill : SkillBase
{
    [SerializeField] private Transform attackPoint;

    public MeleeAttackSkill(Transform attackPoint)
    {
        this.attackPoint = attackPoint;
    }

    protected override void Awake()
    {
        base.Awake();
        KeySkill = 'J'; // Khởi tạo đòn đánh thường mặc định là phím J (Dùng nháy đơn ' ' cho kiểu char)
    }

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Attack);
        animator.PlaySkill(this, "isAttack");

        // Khởi chạy Coroutine tự động dọn dẹp trạng thái sau khi chém xong (Ví dụ: chiêu kéo dài 0.3 giây)
        StartCoroutine(AutoResetAttackRoutine(0.5f));
    }

    private IEnumerator AutoResetAttackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
    
        var statusMachine = GetComponent<CharacterStatusMachine>();
        
        // ✅ Luôn tắt animator flag (phòng trường hợp stuck)
        animator.FinishAttack();
        
        // Chỉ đổi status nếu vẫn đang Attack (tránh đè lên Hurt/Idle)
        if (statusMachine.CurrentState == CharacterStatus.Attack)
        {
            statusMachine.ChangeStatus(CharacterStatus.Idle);
        }
    }

    public override void ApplyEffect()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            targetLayer
        );

        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<IHealth>().ChangeHealth(currentDamageData);
            hit.GetComponent<IKnockbackable>().ApplyKnockback(
                    transform,
                    currentDamageData.EffectForce,
                    currentDamageData.EffectTime,
                    currentDamageData.EffectDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public override float Evaluate(AIContext context)
    {
        if (!base.CanUse()) return 0;

        float distance = context.DistanceToPlayer;
        float score = 0;

        // ✅ AMPLIFIED: Base score tăng từ 60 → 80 để gap lớn hơn
        if (distance <= 1.5f)
        {
            // Khoảng cách càng gần, điểm càng cao
            score = 80 * (1.5f - distance) / 1.5f; // Max = 80 ở distance = 0
        }
        else
        {
            return 0; // Quá xa, không dùng melee
        }

        // Bonus nếu Player aggressive (đánh nhau cận chiến thì melee tốt)
        score += context.PlayerAggression * 20;

        // Penalty nếu Player defensive (hay dash, melee khó trúng)
        score -= context.PlayerDefense * 15;

        // Bonus nếu Boss HP cao (tự tin đấu cận)
        if (context.BossHPPercent > 0.7f)
            score += 15;

        return Mathf.Max(score, 0);
    }
}
