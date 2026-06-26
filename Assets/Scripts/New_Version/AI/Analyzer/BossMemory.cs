using System.Collections.Generic;
using UnityEngine;

public class BossMemory : MonoBehaviour
{
    // === RAW DATA ===
    public int PlayerAttackCount { get; private set; }
    public int PlayerDashCount { get; private set; }
    public int PlayerHealCount { get; private set; }
    public int PlayerAOECount { get; private set; }
    public float CombatTime { get; private set; }

    // === SMART MEMORY (Time-windowed tracking) ===
    [Header("Memory Settings")]
    [SerializeField] private float shortTermWindow = 10f; // Nhớ 10 giây gần nhất
    [SerializeField] private float memoryDecayRate = 0.95f; // Độ suy giảm memory (0.95 = giữ 95% mỗi giây)
    
    private Queue<ActionRecord> recentActions = new Queue<ActionRecord>();
    private Dictionary<SkillEnum, int> shortTermCounts = new Dictionary<SkillEnum, int>();
    
    // === PATTERN DETECTION ===
    private float lastActionTime = 0f;
    private float burstThreshold = 0.3f; // 3 skill trong 0.3s = burst
    private int burstCounter = 0;
    private bool isBursting = false;

    // === WEIGHTED METRICS (hành vi gần đây quan trọng hơn) ===
    private float weightedAttackScore = 0f;
    private float weightedDashScore = 0f;
    private float weightedHealScore = 0f;
    private float weightedAOEScore = 0f;

    private struct ActionRecord
    {
        public SkillEnum skillType;
        public float timestamp;
        
        public ActionRecord(SkillEnum type, float time)
        {
            skillType = type;
            timestamp = time;
        }
    }

    private void Awake()
    {
        // Initialize short-term tracking
        shortTermCounts[SkillEnum.Attack] = 0;
        shortTermCounts[SkillEnum.Dash] = 0;
        shortTermCounts[SkillEnum.Heal] = 0;
        shortTermCounts[SkillEnum.AOE] = 0;
    }

    private void Update()
    {
        if (!WaitingSceneUI.IsBattleStarted) return;
        
        CombatTime += Time.deltaTime;
        
        // Decay weighted scores over time (hành vi cũ dần mất tác động)
        float decayFactor = Mathf.Pow(memoryDecayRate, Time.deltaTime);
        weightedAttackScore *= decayFactor;
        weightedDashScore *= decayFactor;
        weightedHealScore *= decayFactor;
        weightedAOEScore *= decayFactor;
        
        // Clean up old actions outside time window
        CleanupOldActions();
        
        // Reset burst if too long since last action
        if (Time.time - lastActionTime > burstThreshold * 2)
        {
            burstCounter = 0;
            isBursting = false;
        }
    }

    // === REGISTER ACTIONS ===
    public void RegisterAttack()
    {
        PlayerAttackCount++;
        RecordAction(SkillEnum.Attack);
        weightedAttackScore += 1.0f;
    }

    public void RegisterDash()
    {
        PlayerDashCount++;
        RecordAction(SkillEnum.Dash);
        weightedDashScore += 1.0f;
    }

    public void RegisterHeal()
    {
        PlayerHealCount++;
        RecordAction(SkillEnum.Heal);
        weightedHealScore += 1.0f;
    }

    public void RegisterAOE()
    {
        PlayerAOECount++;
        RecordAction(SkillEnum.AOE);
        weightedAOEScore += 1.0f;
    }

    private void RecordAction(SkillEnum skillType)
    {
        float currentTime = Time.time;
        
        // Add to recent actions queue
        recentActions.Enqueue(new ActionRecord(skillType, currentTime));
        
        // Update short-term counts
        if (shortTermCounts.ContainsKey(skillType))
            shortTermCounts[skillType]++;
        
        // Burst detection
        if (currentTime - lastActionTime < burstThreshold)
        {
            burstCounter++;
            if (burstCounter >= 3)
                isBursting = true;
        }
        else
        {
            burstCounter = 1;
            isBursting = false;
        }
        
        lastActionTime = currentTime;
    }

    private void CleanupOldActions()
    {
        float cutoffTime = Time.time - shortTermWindow;
        
        while (recentActions.Count > 0 && recentActions.Peek().timestamp < cutoffTime)
        {
            ActionRecord old = recentActions.Dequeue();
            if (shortTermCounts.ContainsKey(old.skillType))
                shortTermCounts[old.skillType]--;
        }
    }

    // === SMART METRICS (Normalized [0, 1]) ===
    
    /// <summary>
    /// Aggression = % của Attack trong tổng hành động tấn công (Attack + AOE)
    /// Normalize bằng sigmoid để smooth
    /// </summary>
    public float GetAggressionLevel()
    {
        int totalActions = GetTotalActions();
        if (totalActions < 3) return 0.5f; // Chưa đủ data, trả về neutral
        
        // Tính % của Attack trong tổng offensive actions
        int offensiveActions = PlayerAttackCount + PlayerAOECount;
        if (offensiveActions == 0) return 0f;
        
        float attackRatio = (float)PlayerAttackCount / offensiveActions;
        
        // Bonus nếu attack frequency cao (tấn công liên tục)
        float attackFrequency = weightedAttackScore / Mathf.Max(CombatTime, 5f);
        
        // Combine ratio + frequency
        float aggression = attackRatio * 0.6f + Mathf.Clamp01(attackFrequency) * 0.4f;
        
        // Normalize bằng sigmoid để smooth [0, 1]
        return Sigmoid(aggression * 2 - 1); // Map [0,1] -> [-1,1] -> sigmoid
    }

    /// <summary>
    /// Defensive = % của Dash trong tổng hành động
    /// </summary>
    public float GetDefensiveLevel()
    {
        int totalActions = GetTotalActions();
        if (totalActions < 3) return 0.3f; // Default medium defensive
        
        // % Dash trong tổng hành động
        float dashRatio = (float)PlayerDashCount / totalActions;
        
        // Bonus nếu dash trong short-term window cao (đang kite)
        int shortTermTotal = GetShortTermTotalActions();
        float shortTermDashRatio = shortTermTotal > 0 ? 
            (float)shortTermCounts[SkillEnum.Dash] / shortTermTotal : 0f;
        
        // Recent behavior quan trọng hơn
        float defensive = dashRatio * 0.4f + shortTermDashRatio * 0.6f;
        
        return Mathf.Clamp01(defensive * 2); // Amplify và clamp
    }

    /// <summary>
    /// AOE Level = Tần suất dùng AOE + xét tỷ lệ với total actions
    /// </summary>
    public float GetAOELevel()
    {
        int totalActions = GetTotalActions();
        if (totalActions < 3) return 0.2f;
        
        // % AOE trong tổng hành động
        float aoeRatio = (float)PlayerAOECount / totalActions;
        
        // Frequency (skill/second)
        float aoeFrequency = weightedAOEScore / Mathf.Max(CombatTime, 5f);
        
        // Combine
        float aoeLevel = aoeRatio * 0.5f + Mathf.Clamp01(aoeFrequency * 2) * 0.5f;
        
        return Mathf.Clamp01(aoeLevel * 1.5f); // Amplify
    }

    /// <summary>
    /// Healing Level = Tần suất heal (quan trọng vì heal ít nhưng impact cao)
    /// </summary>
    public float GetHealingLevel()
    {
        if (CombatTime < 5f) return 0f; // Chưa đủ thời gian đánh giá
        
        // Mỗi lần heal có trọng số cao
        float healFrequency = weightedHealScore / Mathf.Max(CombatTime, 10f);
        
        // Normalize: 1 heal mỗi 10s = 0.5, 1 heal mỗi 5s = 1.0
        return Mathf.Clamp01(healFrequency * 10f);
    }

    // === PATTERN DETECTION ===
    
    /// <summary>
    /// Phát hiện Player đang burst (spam skill liên tục)
    /// </summary>
    public bool IsBursting()
    {
        return isBursting;
    }

    /// <summary>
    /// Phát hiện Player đang kite (dash nhiều + giữ khoảng cách)
    /// </summary>
    public bool IsKiting()
    {
        // Kite = Dash chiếm > 40% hành động gần đây
        int shortTermTotal = GetShortTermTotalActions();
        if (shortTermTotal < 5) return false;
        
        float recentDashRatio = (float)shortTermCounts[SkillEnum.Dash] / shortTermTotal;
        return recentDashRatio > 0.4f;
    }

    /// <summary>
    /// Phát hiện Player đang hit & run (Attack + Dash xen kẽ)
    /// </summary>
    public bool IsHitAndRun()
    {
        int shortTermTotal = GetShortTermTotalActions();
        if (shortTermTotal < 6) return false;
        
        int attacks = shortTermCounts[SkillEnum.Attack];
        int dashes = shortTermCounts[SkillEnum.Dash];
        
        // Hit & Run = Attack và Dash đều có, tỷ lệ gần nhau
        return attacks >= 2 && dashes >= 2 && Mathf.Abs(attacks - dashes) <= 2;
    }

    // === UTILITY ===
    
    private int GetTotalActions()
    {
        return PlayerAttackCount + PlayerDashCount + PlayerHealCount + PlayerAOECount;
    }

    private int GetShortTermTotalActions()
    {
        int total = 0;
        foreach (var count in shortTermCounts.Values)
            total += count;
        return total;
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    // === DEBUG ===
    public void LogMemoryState()
    {
        Debug.Log($"[Memory] Total: {GetTotalActions()} | Aggression: {GetAggressionLevel():F2} | " +
                  $"Defensive: {GetDefensiveLevel():F2} | AOE: {GetAOELevel():F2} | " +
                  $"Heal: {GetHealingLevel():F2} | Burst: {IsBursting()} | Kite: {IsKiting()}");
    }
}