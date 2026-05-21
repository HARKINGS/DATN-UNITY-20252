using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private BossMemory memory;

    [SerializeField] private Transform Player;
    private CharacterDetection characterDetection;
    private Vector2 PlayerDirection;
    private Rigidbody2D rb;
    private CharacterStatus characterState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skillCaster = GetComponent<SkillCaster>();
        movement = GetComponent<CharacterMovement>();
        memory = GetComponent<BossMemory>();
        characterDetection = GetComponent<CharacterDetection>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (CheckCharacter() != null)
        {
            //Debug.Log("Find Player");
            PlayerDirection = GetDirection();
            Think();
            movement.Move(PlayerDirection);
        } 
        else movement.Move(Vector2.zero);
    }

    private Vector2 GetDirection()
    {
        return (Player.position - transform.position).normalized;
    }

    private Transform CheckCharacter()
    {
        Collider2D[] hits = characterDetection.DetectCharacter();
        if (hits.Length > 0)
            return Player = hits[0].transform;
        return Player = null;
    }

    private void Think()
    {
        SkillBase bestSkill = null;
        float bestScore = -1;

        List<SkillBase> skills = skillCaster.GetSkills();
        float aggression = memory.GetAggressionLevel();
        float defensive = memory.GetDefensiveLevel();

        AIContext context = new AIContext
        (
            (Player.position - transform.position).magnitude,
            BossHPPercent: movement.GetHealth().GetHealthPercent(),
            PlayerAggression: aggression,
            PlayerDefense: defensive,
            Player: Player
        );

        foreach (SkillBase skill in skills)
        {
            float score = skill.Evaluate(context);

            if (score > bestScore)
            {
                bestScore = score;
                bestSkill = skill;
            }
        }

        skillCaster.Execute(bestSkill.SkillType);
        //Debug.Log("The Best Skill: " + bestSkill.SkillType);
    }
}
