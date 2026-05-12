using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackSkill : SkillBase
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask targetLayer;

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        animator.PlayAttack();
    }

    public override void ApplyEffect()
    {
        Debug.Log("Melee Attack!");
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
                    currentDamageData.KnockBackForce,
                    currentDamageData.KnockBackTime,
                    currentDamageData.KnockBackDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
