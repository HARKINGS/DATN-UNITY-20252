using System.Collections;
using UnityEngine;

public class AOELightingSkill : SkillBase
{
    [Header("Spark Variables")]
    public GameObject sparkFXPrefab;
    public GameObject borderLightFXPrefab;
    protected override void Awake()
    {
        base.Awake();
        KeySkill = 'K'; // Khởi tạo đòn đánh thường mặc định là phím J (Dùng nháy đơn ' ' cho kiểu char)
    }

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Cast);
        Debug.Log("Current Status is: " + GetComponent<CharacterStatusMachine>().CurrentState);
        animator.PlaySkill(this, "isCasting");

        // Khởi chạy Coroutine tự động dọn dẹp trạng thái sau khi chém xong (Ví dụ: chiêu kéo dài 0.3 giây)
        StartCoroutine(AutoResetAOERoutine(0.3f));
    }

    private IEnumerator AutoResetAOERoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Kiểm tra nếu lúc này Boss vẫn đang Cast thì mới trả về Idle (tránh đè lên trạng thái Hurt)
        var statusMachine = GetComponent<CharacterStatusMachine>();

        animator.FinishCast(); // Tắt cờ isCasting trong Animator
        
        if (statusMachine.CurrentState == CharacterStatus.Cast)
        {
            statusMachine.ChangeStatus(CharacterStatus.Idle); // Đưa logic về Move
            Debug.Log("AOE Cast finished, returning to Move state.");
        }
    }

    public override void ApplyEffect()
    {
        //Debug.Log("AOE Attack!");

        if (borderLightFXPrefab != null)
        {
            GameObject newFx = Instantiate(borderLightFXPrefab, transform.position, Quaternion.identity);
            Destroy(newFx, 1f);
        }

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            targetLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            CharacterHealth health = enemy.GetComponent<CharacterHealth>();
            if (health != null)
            {
                health.ChangeHealth(currentDamageData);

                if (sparkFXPrefab != null)
                {
                    GameObject newFx = Instantiate(sparkFXPrefab, health.hitPoint.position, Quaternion.identity);
                    Destroy(newFx, 1f);
                }
            }
        }
    }

    public override float Evaluate(AIContext context)
    {
        if (!base.CanUse()) return 0;

        float distance = context.DistanceToPlayer;
        float score = 0;

        // Base score: hiệu quả ở khoảng cách trung bình
        if (distance <= 5.5f && distance >= 1.0f)
        {
            // Tối ưu ở khoảng cách 2-4f
            if (distance >= 2f && distance <= 4f)
                score = 70;
            else
                score = 50;
        }
        else
        {
            return 0; // Quá xa hoặc quá gần
        }

        // BONUS LỚN nếu Player defensive (hay dash, AOE bắt khu vực rộng)
        score += context.PlayerDefense * 40;

        // Bonus nếu Player aggressive (AOE punish khi Player lao vào)
        score += context.PlayerAggression * 25;

        // Bonus nếu Player hay dùng AOE (đáp trả bằng AOE)
        score += context.PlayerAOEAgression * 30;

        // Bonus nếu đầu trận (CombatTime < 15s) -> AOE surprise
        if (context.CombatTime < 15f)
            score += 20;

        // ✨ PATTERN BONUS: Nếu Player đang burst (spam skill) -> AOE punish!
        if (context.PlayerIsBursting)
            score += 35;

        // ✨ PATTERN BONUS: Nếu Player đang kite -> AOE bắt khu vực
        if (context.PlayerIsKiting)
            score += 30;

        return score;
    }
}
