using System.Collections.Generic;
using UnityEngine;

public class SkillCaster : MonoBehaviour
{
    public SpellUIManager spellUIManager;

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

    private void Start()
    {
        if(spellUIManager != null) spellUIManager.ShowSkills(skills);
    }

    public void Execute(SkillEnum type)
    {
        if (!skillMap.ContainsKey(type))
            return;

        SkillBase CurrentSkill = skillMap[type];

        if (!CurrentSkill.CanUse()) return;

        DamageData damageData = BuildContext();

        CurrentSkill.Execute(damageData);

        if(spellUIManager != null) 
            spellUIManager.HighlightSkill(CurrentSkill);
    }

    private void HighlightSkill(SkillBase CurrentSkill)
    {
        if (CurrentSkill != null)
        {
            spellUIManager.HighlightSkill(CurrentSkill);
        }
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
