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
    private CharacterStatusMachine StatusMachine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StatusMachine = GetComponent<CharacterStatusMachine>();
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
            CharacterStatus currentStatus = StatusMachine.CurrentState;

            // Nếu đang bị thương, choáng hoặc đang ra chiêu, không làm gì cả
            if (currentStatus == CharacterStatus.Hurt ||
                currentStatus == CharacterStatus.Stun ||
                movement.GetAnimation().CheckStatus("isCasting") || 
                movement.GetAnimation().CheckStatus("isAttack")) // Kiểm tra thông qua Animator hoặc Status
            {
                Debug.Log("currentStatus: " + currentStatus);
                Debug.Log("Check Status: " + movement.GetAnimation().CheckStatus("isCasting") + " - " + movement.GetAnimation().CheckStatus("isAttack"));
                movement.Move(Vector2.zero);
                return;
            }

            PlayerDirection = GetDirection();

            // Cooldown cho việc suy nghĩ, không gọi Think() mỗi khung hình
            // Hoặc chỉ Think() khi đang ở trạng thái Idle/Move
            //if (movement.GetAnimation().CheckStatus("isIdle") || 
            //    movement.GetAnimation().CheckStatus("isChasing"))
            //{
                Debug.Log("Boss is thinking...");
                Think();
            //}

            movement.Move(PlayerDirection);
        }
        else
        {
            movement.Move(Vector2.zero);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            StatusMachine.ChangeStatus(CharacterStatus.Idle);
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
        CharacterStatus currentStatus = StatusMachine.CurrentState;

        if (currentStatus == CharacterStatus.Hurt || currentStatus == CharacterStatus.Stun)
            return;

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
        Debug.Log("The Best Skill: " + bestSkill.SkillType);
    }
}
