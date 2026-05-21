using UnityEngine;

public class HealSkill : SkillBase
{
    [Header("Heal Variables")]
    public GameObject healFXPrefab;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform;
    }

    private void Update()
    {
        playerTransform = transform;
    }

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Casting);
        animator.PlaySkill(this, "isCasting");
    }

    public override void ApplyEffect()
    {
        Debug.Log("Heal Apply!");

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
        if (context.BossHPPercent < 0.3f && base.CanUse())
            return 100;

        return 0;
    }
}
