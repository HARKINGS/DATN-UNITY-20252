using UnityEngine;

public class HealSkill : SkillBase
{
    [Header("Heal Variables")]
    public GameObject healFXPrefab;

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Cast);
        animator.PlaySkill(this, "isCasting");
    }

    public override void ApplyEffect()
    {
        //Debug.Log("Heal Apply!");

        CharacterHealth health = GetComponent<CharacterHealth>();
        if (health != null)
        {
            health.Heal(currentDamageData);

            if (healFXPrefab != null)
            {
                GameObject newFx = Instantiate(healFXPrefab, health.hitPoint.position, Quaternion.identity);
                Destroy(newFx, 1f);
            }
        }
    }

    public override float Evaluate(AIContext context)
    {
        if (context.BossHPPercent < 0.5f && base.CanUse())
            return 100;

        return 0;
    }
}
