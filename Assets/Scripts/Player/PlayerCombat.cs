using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    //public float weaponRange;
    //public float knockbackForce = 50;
    //public float knockbackTime = .15f;
    //public float stunTime = .3f;
    public LayerMask enemyLayer;
    //public int damage = 1;
    public StatsManager statsManager;

    public Animator anim;

    public float cooldown = 1f;
    private float timer;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttack", true);
            timer = cooldown;
        }
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            StatsManager.Instance.weaponRange,
            enemyLayer
        );

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-StatsManager.Instance.damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, 
                StatsManager.Instance.knockbackForce, 
                StatsManager.Instance.knockbackTime, 
                StatsManager.Instance.stunTime);
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttack", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, statsManager.weaponRange);
    }
}
