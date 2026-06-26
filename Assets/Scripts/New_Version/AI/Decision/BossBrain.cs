using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private BossMemory memory;
    [SerializeField] private CharacterAnalyzer analyzer;

    [SerializeField] private Transform Player;
    private CharacterDetection characterDetection;
    private Vector2 PlayerDirection;
    private CharacterStatusMachine StatusMachine;

    // Movement Strategy
    [Header("Movement Settings")]
    [SerializeField] private float safeDistance = 4.0f; // Khoảng cách an toàn
    [SerializeField] private float retreatDistance = 0.8f; // Khoảng cách quá gần cần lùi
    
    
    // Circle movement state
    private Vector2 currentCircleDirection;
    private float circleDirectionChangeTime = 0f;
    private float circleDirectionDuration = 2f; // Giữ hướng vòng 2 giây trước khi đổi

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        analyzer = GetComponent<CharacterAnalyzer>();
        StatusMachine = GetComponent<CharacterStatusMachine>();
        skillCaster = GetComponent<SkillCaster>();
        movement = GetComponent<CharacterMovement>();
        memory = analyzer.GetBossMemory();
        characterDetection = GetComponent<CharacterDetection>();
        
        // Initialize circle direction
        currentCircleDirection = Vector2.right;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!WaitingSceneUI.IsBattleStarted)
        {
            movement.Move(Vector2.zero);
            return;
        }

        if (CheckCharacter() != null)
        {
            CharacterStatus currentStatus = StatusMachine.CurrentState;

            // ✅ ROLLBACK: Block movement khi Hurt/Stun (do CharacterMovement đã disable)
            bool isExecutingSkill = movement.GetAnimation().CheckStatus("isCasting") || 
                                   movement.GetAnimation().CheckStatus("isAttack");
            
            bool cannotMove = isExecutingSkill || 
                             currentStatus == CharacterStatus.Hurt || 
                             currentStatus == CharacterStatus.Stun;

            if (cannotMove)
            {
                // Dừng di chuyển khi đang cast skill hoặc bị knockback
                movement.Move(Vector2.zero);
                return;
            }

            PlayerDirection = GetDirection();
            
            // AI Think - chọn skill
            Think();

            // Movement Strategy - di chuyển thông minh
            Vector2 moveDirection = DecideMovement();
            movement.Move(moveDirection);
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
        float aoeAggression = memory.GetAOELevel();
        float healFrequency = memory.GetHealingLevel();

        AIContext context = new AIContext
        (
            DistanceToPlayer: Vector2.Distance(transform.position, Player.position),
            BossHPPercent: movement.GetHealth().GetHealthPercent(),
            PlayerAggression: aggression,
            PlayerAOEAgression: aoeAggression,
            PlayerDefense: defensive,
            PlayerHealFrequency: healFrequency,
            Player: Player,
            CombatTime: memory.CombatTime,
            PlayerIsBursting: memory.IsBursting(),
            PlayerIsKiting: memory.IsKiting(),
            PlayerIsHitAndRun: memory.IsHitAndRun()
        );

        // Debug patterns
        if (context.PlayerIsBursting || context.PlayerIsKiting || context.PlayerIsHitAndRun)
        {
            Debug.Log($"[AI Pattern] Burst: {context.PlayerIsBursting} | Kite: {context.PlayerIsKiting} | Hit&Run: {context.PlayerIsHitAndRun}");
        }

        // Đánh giá từng skill
        foreach (SkillBase skill in skills)
        {
            float score = skill.Evaluate(context);

            // Debug log để kiểm tra
            if (score > 0)
            {
                Debug.Log($"[AI] {skill.SkillType}: Score = {score:F1} | Distance = {context.DistanceToPlayer:F1}");
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestSkill = skill;
            }
        }

        if (bestSkill == null || bestScore <= 0f)
        {
            Debug.Log("[AI] Không tìm được skill phù hợp hoặc đang cooldown.");
            return;
        }

        // Thực thi skill tốt nhất
        Debug.Log($"[AI] ✓ Executing: {bestSkill.SkillType} (Score: {bestScore:F1})");
        skillCaster.Execute(bestSkill.SkillType);
    }

    // Movement Strategy - Di chuyển thông minh dựa trên context
    private Vector2 DecideMovement()
    {
        float distance = Vector2.Distance(transform.position, Player.position);
        float aggression = memory.GetAggressionLevel();
        float bossHP = movement.GetHealth().GetHealthPercent();

        // PRIORITY 1: Boss HP thấp (<40%) → RETREAT (Lùi ra xa để heal)
        if (bossHP < 0.4f)
        {
            if (distance < safeDistance)
            {
                // Lùi thẳng ra xa Player
                Debug.Log($"[Movement] RETREAT | Distance: {distance:F1} < {safeDistance} | HP: {bossHP:P0}");
                return -PlayerDirection; // Full speed lùi
            }
            else
            {
                // Đã đủ xa, kite để chờ cooldown heal
                return GetCircleMovement();
            }
        }

        // PRIORITY 3: Player rất aggressive + đang ở gần → CIRCLE KITE
        if (aggression > 0.5f && distance < 3f)
        {
            Debug.Log($"[Movement] CIRCLE KITE | Aggression: {aggression:F2} | Distance: {distance:F1}");
            return GetCircleMovement();
        }

        // PRIORITY 4: Quá gần (<0.8f) → LÙI NHẸ (tránh bị dồn vào góc)
        if (distance < retreatDistance)
        {
            Debug.Log($"[Movement] RETREAT LIGHT | Distance: {distance:F1} < {retreatDistance}");
            return -PlayerDirection * 0.7f; // Lùi chậm
        }

        // PRIORITY 5: Khoảng cách optimal (0.9f - 1.5f) → DỪNG LẠI (để attack)
        if (distance >= 0.9f && distance <= 1.5f)
        {
            return Vector2.zero; // Dừng để skill execute
        }

        // PRIORITY 6: Quá xa (>1.5f) → TIẾN VÀO
        if (distance > 1.5f)
        {
            return PlayerDirection; // Full speed tiến
        }

        // Default: đứng yên
        return Vector2.zero;
    }

    // Di chuyển vòng quanh Player - CỐ ĐỊNH HƯỚNG trong thời gian dài
    private Vector2 GetCircleMovement()
    {
        // Chỉ đổi hướng sau mỗi 2 giây
        if (Time.time - circleDirectionChangeTime > circleDirectionDuration)
        {
            // Vector vuông góc với direction tới Player
            Vector2 perpendicular = new Vector2(-PlayerDirection.y, PlayerDirection.x);
            
            // Random trái hoặc phải MỘT LẦN
            if (Random.value > 0.5f)
                perpendicular = -perpendicular;
            
            // Lưu hướng mới
            currentCircleDirection = perpendicular;
            circleDirectionChangeTime = Time.time;
            
            Debug.Log($"[Movement] Circle direction changed: {currentCircleDirection}");
        }
        
        // Mix giữa di chuyển vòng (70%) và tiến vào (30%) để không đi xa quá
        Vector2 finalDirection = (currentCircleDirection * 0.7f + PlayerDirection * 0.3f).normalized;
        
        return finalDirection;
    }
}
