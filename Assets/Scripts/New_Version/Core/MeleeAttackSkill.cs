using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackSkill : MonoBehaviour, ISkill
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask targetLayer;

    public void Execute(DamageData damageData)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            targetLayer
        );

        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<IHealth>().ChangeHealth(damageData);
            hit.GetComponent<IKnockbackable>().ApplyKnockback(
                    damageData.HitDirection,
                    damageData.KnockBackForce,
                    damageData.KnockBackDuration);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
