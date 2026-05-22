using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    [SerializeField] private List<SkillBase> skills;
    private Dictionary<SkillEnum, SkillBase> skillMap;
    private CharacterStats stats;

    public List<SkillBase> GetSkills() { return skills; }

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();

        skillMap = new Dictionary<SkillEnum, SkillBase>();

        foreach (SkillBase skill in skills)
        {
            skillMap.Add(skill.SkillType, skill);
        }
    }
    public void Execute(SkillEnum type)
    {
        //Debug.Log(type);
        if (!skillMap.ContainsKey(type))
            return;

        //Debug.Log(type);
        SkillBase skill = skillMap[type];

        if (!skill.CanUse()) return;

        DamageData damageData = BuildContext();

        skill.Execute(damageData);
    }


    private DamageData BuildContext()
    {
        return new DamageData
                {
                    EffectForce = stats.EffectForce,
                    EffectTime = stats.EffectTime,
                    EffectDuration = stats.EffectDuration,
                    HitDirection = transform.right,
                    Attacker = gameObject
                };
    }    
}
