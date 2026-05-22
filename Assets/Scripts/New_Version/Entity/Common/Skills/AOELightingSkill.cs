using UnityEngine;

public class AOELightingSkill : SkillBase
{
    [Header("Spark Variables")]
    public GameObject sparkFXPrefab;
    public GameObject borderLightFXPrefab;

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Cast);
        Debug.Log("Current Status is: " + GetComponent<CharacterStatusMachine>().CurrentState);
        animator.PlaySkill(this, "isCasting");
    }

    public override void ApplyEffect()
    {
        //Debug.Log("AOE Attack!");

        if (borderLightFXPrefab != null)
        {
            GameObject newFx = Instantiate(borderLightFXPrefab, transform.position, Quaternion.identity);
            Destroy(newFx, 1f);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            targetLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            CharacterHealth health = enemy.GetComponent<CharacterHealth>();
            if (health != null)
            {
                health.ChangeHealth(currentDamageData);

                if (sparkFXPrefab != null)
                {
                    GameObject newFx = Instantiate(sparkFXPrefab, health.hitPoint.position, Quaternion.identity);
                    Destroy(newFx, 1f);
                }
            }
        }
    }

    public override float Evaluate(AIContext context)
    {
        float score = 10;

        //Debug.Log("AOE Evaluate: Distance to Player = " + context.DistanceToPlayer);

        if (context.DistanceToPlayer <= 5.5f && base.CanUse())
            score += 60;

        return score;
    }
}
