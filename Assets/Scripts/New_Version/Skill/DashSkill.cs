using Unity.Jobs;
using Unity.VisualScripting;
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

    private void Start()
    {
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
        ApplyEffect();
    }

    public override void ApplyEffect()
    {
        Debug.Log("Apply Dash!");
        if (dashFXPrefab != null)
        {
            GameObject newFx = Instantiate(dashFXPrefab, health.hitPoint.position, Quaternion.identity);
            Destroy(newFx, dashEffectDuration);
        }

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
}
