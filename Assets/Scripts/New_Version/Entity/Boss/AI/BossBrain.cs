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
        if (!WaitingSceneUI.IsBattleStarted)
        {
            movement.Move(Vector2.zero);
            return;
        }

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
                //Debug.Log("Check Status: " + movement.GetAnimation().CheckStatus("isCasting") + " - " + movement.GetAnimation().CheckStatus("isAttack"));
                movement.Move(Vector2.zero);
                return;
            }

            PlayerDirection = GetDirection();

            //Debug.Log("Boss is thinking...");
            Think();

            movement.Move(PlayerDirection);
        }
        else
        {
            movement.Move(Vector2.zero);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 1. Đưa trạng thái Logic về Idle
            StatusMachine.ChangeStatus(CharacterStatus.Idle);

            // 2. QUAN TRỌNG: Đưa trạng thái hình ảnh (Animator) về an toàn
            CharacterAnimation bossAnim = movement.GetAnimation();
            if (bossAnim != null)
            {
                bossAnim.ResetAnimation();
                bossAnim.AnimationEvent_EndSkill(); // Xóa skill đang cast dở
                bossAnim.FinishCast();              // Tắt isCasting
                bossAnim.FinishAttack();            // Tắt isAttack
            }

            // 3. Đưa vận tốc vật lý về 0 ngay lập tức
            movement.Move(Vector2.zero);
        }
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
            DistanceToPlayer: Vector2.Distance(transform.position, Player.position),
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

        if (bestSkill == null || bestScore <= 0f)
        {
            Debug.Log("AI chưa tìm được kỹ năng phù hợp hoặc tất cả đang hồi chiêu.");
            return;
        }

        // 4. CHẮC CHẮN tung chiêu thành công vì đã được lọc CanUse() từ trên
        Debug.Log("The Best Skill Executed: " + bestSkill.SkillType);
        skillCaster.Execute(bestSkill.SkillType);
    }
}
