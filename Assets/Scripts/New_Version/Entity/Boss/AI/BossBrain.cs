using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private BossMemory memory;
    [SerializeField] private Transform Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillCaster = GetComponent<SkillCaster>();
        movement = GetComponent<CharacterMovement>();
        memory = GetComponent<BossMemory>();
    }

    // Update is called once per frame
    void Update()
    {
        Think();   
    }

    private Vector2 GetDirection()
    {
        return (Player.transform.position - transform.position).normalized;
    }

    private void Think()
    {
        //SkillBase bestSkill = null;

        //float bestScore = -1;

        //List<SkillBase> skills = skillCaster.GetSkills();

        //foreach (SkillBase skill in skills)
        //{
        //    float score = skill.Evaluate
        //    (
        //        new AIContext
        //        (
        //            GetDirection().magnitude,
        //            BossHPPercent: movement.GetHealth().GetHealthPercent(),
        //            PlayerAggression: memory.GetAggressionLevel(),
        //            PlayerDefense: memory.GetDefensiveLevel(),
        //            Player: Player
        //        )            
        //    );

        //    if (score > bestScore)
        //    {
        //        bestScore = score;
        //        bestSkill = skill;
        //    }
        //}

        //float aggression = memory.GetAggressionLevel();
        //float defensive = memory.GetDefensiveLevel();
        //if (aggression > 0.5f)
        //{
        //    skillCaster.Execute(SkillEnum.Attack);
        //}
        //else if (defensive > 0.5f)
        //{
        //    Vector2 direction = GetDirection();
        //    movement.Move(direction);
        //}
        //else
        //{
        //    skillCaster.Execute(SkillEnum.Idle);
        //}
    }
}
