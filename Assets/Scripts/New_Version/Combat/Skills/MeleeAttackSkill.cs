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
        //StartCoroutine(AutoResetAttackRoutine(0.5f));
    }

    private IEnumerator AutoResetAttackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Kiểm tra nếu lúc này Boss vẫn đang Attack thì mới trả về Idle (tránh đè lên trạng thái Hurt)
        var statusMachine = GetComponent<CharacterStatusMachine>();
        if (statusMachine.CurrentState == CharacterStatus.Attack)
        {
            animator.FinishAttack(); // Tắt cờ isAttack trong Animator
            statusMachine.ChangeStatus(CharacterStatus.Move); // Đưa logic về Move
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
        //Debug.Log("Melee Can use: " + base.CanUse());
        //Debug.Log("Distance To Player: " + context.DistanceToPlayer);

        if (context.DistanceToPlayer <= 1.1f && base.CanUse())
            return 90;
        return 0;
    }
}
