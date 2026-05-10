using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    [SerializeField] private MeleeAttackSkill meleeAttack;
    [SerializeField] private CharacterAnimation animator;

    [Header("Combat Stats")]
    [SerializeField] private CharacterStats stats;

    //[Header("Combat Stats")]
    //[SerializeField] private int damage = 10;
    //[SerializeField] private float knockbackForce = 10f;
    //[SerializeField] private float knockbackDuration = 0.2f;

    public void Attack()
    {
        animator.PlayAttack();

        DamageData damageData =
            new DamageData
            {
                Damage = stats.Damage,
                KnockBackForce = stats.KnockbackForce,
                KnockBackDuration = stats.KnockbackDuration,
                HitDirection = transform.right,
                Attacker = gameObject
            };

        meleeAttack.Execute(damageData);
    }
}