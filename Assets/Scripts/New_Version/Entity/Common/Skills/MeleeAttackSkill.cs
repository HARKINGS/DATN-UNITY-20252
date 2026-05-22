using UnityEngine;

public class MeleeAttackSkill : SkillBase
{
    [SerializeField] private Transform attackPoint;

    public MeleeAttackSkill(Transform attackPoint)
    {
        this.attackPoint = attackPoint;
    }    

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Attack);
        animator.PlaySkill(this, "isAttack");
    }

    public override void ApplyEffect()
    {
        //Debug.Log("Melee Attack!");
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
        if (context.DistanceToPlayer <= 1f && base.CanUse())
            return 90;
        return 0;
    }
}
