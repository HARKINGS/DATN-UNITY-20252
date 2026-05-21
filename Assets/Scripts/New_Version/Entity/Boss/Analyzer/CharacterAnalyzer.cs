using UnityEngine;

public class CharacterAnalyzer : MonoBehaviour
{
    [SerializeField] private BossMemory bossMemory;

    private void Awake()
    {
        bossMemory = GetComponent<BossMemory>();
    }

    private void OnEnable()
    {
        CombatEvents.OnPlayerSkillUsed += AnalyzeSkill;
    }

    private void OnDisable()
    {
        CombatEvents.OnPlayerSkillUsed -= AnalyzeSkill;
    }

    private void AnalyzeSkill(SkillBase skill)
    {   
        //Debug.Log("Analyzing skill: " + skill.SkillType);
        //Debug.Log("Attacker: " + skill.GetAttacker().name + " with tag: " + skill.GetAttacker().tag);

        if (skill.GetAttacker().tag == "Player")
        {
            switch (skill.SkillType)
            {
                case SkillEnum.Attack: bossMemory.RegisterAttack(); break;
                case SkillEnum.AOE: bossMemory.RegisterAOE(); break;
                case SkillEnum.Dash: bossMemory.RegisterDash(); break;
                case SkillEnum.Heal: bossMemory.RegisterHeal(); break;
            }

            //Debug.Log($"Melee: {bossMemory.PlayerAttackCount}, " +
            //    $"AOE: {bossMemory.PlayerAOECount}, " +
            //    $"Dash: {bossMemory.PlayerDashCount}, " +
            //    $"Heal: {bossMemory.PlayerHealCount}");
        }
    }
}
