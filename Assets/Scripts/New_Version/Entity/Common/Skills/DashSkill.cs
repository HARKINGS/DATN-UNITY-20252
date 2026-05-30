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
    }

    public override float Evaluate(AIContext context)
    {
        if(context.DistanceToPlayer > 5f && base.CanUse())
            return 80; // High priority if player is far and aggressive
        return 0;
    }
}
