using UnityEngine;

public class BossAOESkill : SkillBase
{
    public override void Execute(DamageData damageData)
    {
        base.Execute(damageData);
        Debug.Log("AOE Attack!");
    }
}
