using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillSelector : MonoBehaviour
{
    [SerializeField] private List<SkillBase> skills;

    public SkillBase SelectSkill(BossBehaviorType behavior)
    {
        switch (behavior)
        {
            //case BossBehaviorType.Counter:
            //    return skills.Find(s => s is CounterSkill);

            case BossBehaviorType.AOE:
                return skills.Find(s => s is BossAOESkill);

            default:
                return skills.Find(s => s is BossAOESkill);
        }
    }
}
