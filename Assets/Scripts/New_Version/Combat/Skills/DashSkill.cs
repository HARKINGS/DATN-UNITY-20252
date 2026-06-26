using UnityEngine;

public class DashSkill : SkillBase
{
    private Transform playerTransform;
    private CharacterMovement movement;
    private CharacterHealth health;

    [SerializeField] private float dashSpace = 3.0f;
    [SerializeField] private float dashEffectDuration = 0.5f;
    [SerializeField] private GameObject dashFXPrefab;

    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private LayerMask obstacleLayer;

    override protected void Start()
    {
        base.Start();
        playerTransform = transform;
        health = GetComponent<CharacterHealth>();
        movement = GetComponent<CharacterMovement>();
    }

    protected override void Awake()
    {
        base.Awake();
        KeySkill = ';'; // Khởi tạo đòn đánh thường mặc định là phím J (Dùng nháy đơn ' ' cho kiểu char)
    }

    private void Update()
    {
        playerTransform = transform;
    }

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Dash);

        if (dashFXPrefab != null)
        {
            GameObject newFx = Instantiate(dashFXPrefab, health.hitPoint.position, Quaternion.identity);
            Destroy(newFx, dashEffectDuration);
        }
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        Vector2 dashDirection = movement.getMove();
        Vector2 destination = (Vector2)playerTransform.position + dashDirection * dashSpace;

        Collider2D hit = Physics2D.OverlapCircle(destination, playerRadius, obstacleLayer);
        if (hit != null)
        {
            float step = .1f;
            Vector2 adjustPosition = destination;
            while (hit != null && Vector2.Distance(adjustPosition, playerTransform.position) > 0)
            {
                adjustPosition -= step * dashDirection;
                hit = Physics2D.OverlapCircle(adjustPosition, playerRadius, obstacleLayer);
            }
            destination = adjustPosition;
        } 

        playerTransform.position = destination;
        if(dashDirection != Vector2.zero)
            GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Move);
        else 
            GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Idle);
    }

    public override float Evaluate(AIContext context)
    {
        if (!base.CanUse()) return 0;

        float distance = context.DistanceToPlayer;
        float score = 0;

        // Case 1: Player ở quá xa -> Dash để tiếp cận
        if (distance > 5f)
        {
            score = 80;
            // BONUS nếu Player aggressive (cần đuổi kịp để fight)
            score += context.PlayerAggression * 30;
            // BONUS nếu Player hay AOE (cần dash vào gần để tránh AOE)
            score += context.PlayerAOEAgression * 35;
        }
        // Case 2: Boss HP thấp và cần tạo khoảng cách
        else if (context.BossHPPercent < 0.4f && distance < 3f)
        {
            score = 90; // Dash ra xa để heal
        }
        // Case 3: Player defensive (hay dash) -> Boss cũng dash để đuổi theo
        else if (context.PlayerDefense > 0.6f && distance > 3f)
        {
            score = 70;
        }
        else
        {
            return 0; // Không cần dash
        }

        // ✨ PATTERN BONUS: Nếu Player đang kite -> Dash counter (đuổi theo hoặc thoát)
        if (context.PlayerIsKiting)
        {
            if (distance > 4f)
                score += 40; // Dash vào gần
            else
                score += 25; // Dash ra xa nếu quá gần
        }

        // ✨ PATTERN BONUS: Nếu Player Hit & Run -> Dash đuổi theo
        if (context.PlayerIsHitAndRun && distance > 3f)
            score += 35;

        return score;
    }
}
