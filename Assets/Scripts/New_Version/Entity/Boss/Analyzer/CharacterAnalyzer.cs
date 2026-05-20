using UnityEngine;

public class CharacterAnalyzer : MonoBehaviour
{
    [SerializeField] private BossMemory bossMemory;

    private float combatTimer;

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
        Debug.Log("Analyzing skill: " + skill.SkillType);
        Debug.Log("Attacker is: " + skill.GetAttacker().tag);

        switch (skill.SkillType)
        {
            case SkillEnum.Attack: bossMemory.RegisterAttack(); break;
            case SkillEnum.AOE: bossMemory.RegisterAOE(); break;
            case SkillEnum.Dash: bossMemory.RegisterDash(); break;
            case SkillEnum.Heal: bossMemory.RegisterHeal(); break;
        }

        Debug.Log($"Melee: {bossMemory.PlayerAttackCount}, " +
            $"AOE: {bossMemory.PlayerAOECount}, " +
            $"Dash: {bossMemory.PlayerDashCount}, " +
            $"Heal: {bossMemory.PlayerHealCount}");
    }
}
