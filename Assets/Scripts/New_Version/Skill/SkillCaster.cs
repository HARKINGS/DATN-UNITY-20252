using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    [SerializeField] private List<SkillBase> skills;
    private Dictionary<SkillEnum, SkillBase> skillMap;
    [Header("Combat Stats")]
    [SerializeField] private CharacterStats stats;

    private void Awake()
    {
        skillMap = new Dictionary<SkillEnum, SkillBase>();

        foreach (SkillBase skill in skills)
        {
            skillMap.Add(skill.SkillType, skill);
        }
    }
    public void Execute(SkillEnum type)
    {
        if (!skillMap.ContainsKey(type))
            return;
        
        SkillBase skill = skillMap[type];

        if(!skill.CanUse()) return;

        skill.Execute(BuildContext());
    }  
    
    private DamageData BuildContext()
    {
        return new DamageData
                {
                    KnockBackForce = stats.KnockbackForce,
                    KnockBackTime = stats.KnockbackTime,
                    KnockBackDuration = stats.KnockbackDuration,
                    HitDirection = transform.right,
                    Attacker = gameObject
                };
    }    
}
