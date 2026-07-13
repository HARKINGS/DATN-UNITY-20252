using System.Collections;
using UnityEngine;

public class HealSkill : SkillBase
{
    [Header("Heal Variables")]
    public GameObject healFXPrefab;

    protected override void Awake()
    {
        base.Awake();
        KeySkill = 'L'; // Khởi tạo đòn đánh thường mặc định là phím J (Dùng nháy đơn ' ' cho kiểu char)
    }

    public override void Execute(DamageData damageData)
    {
        damageData.Damage = damage;
        base.Execute(damageData);
        GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Cast);
        animator.PlaySkill(this, "isCasting");

        StartCoroutine(AutoResetHealRoutine(0.3f));
    }

    private IEnumerator AutoResetHealRoutine(float duration)
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
        //Debug.Log("Heal Apply!");

        CharacterHealth health = GetComponent<CharacterHealth>();
        if (health != null)
        {
            health.Heal(currentDamageData);

            if (healFXPrefab != null)
            {
                GameObject newFx = Instantiate(healFXPrefab, health.hitPoint.position, Quaternion.identity);
                Destroy(newFx, 1f);
            }
        }
    }

    public override float Evaluate(AIContext context)
    {
        if (!base.CanUse()) return 0;

        float score = 0;
        float hpPercent = context.BossHPPercent;

        // ✅ AMPLIFIED: Base score tăng từ 150 → 200 để thấy rõ priority sống còn
        if (hpPercent < 0.3f)
            score = 200; // CỰC KỲ CAO - sống còn
        else if (hpPercent < 0.5f)
            score = 120; // Tăng từ 100 → 120
        else if (hpPercent < 0.7f)
            score = 70; // Tăng từ 60 → 70
        else
            return 0; // HP còn nhiều, không cần heal

        // PENALTY nếu Player aggressive (đang bị dồn ép, khó heal)
        score -= context.PlayerAggression * 30;

        // BONUS nếu đã tạo khoảng cách an toàn
        if (context.DistanceToPlayer > 4f)
            score += 40;

        // PENALTY nếu Player ở quá gần (dễ bị interrupt)
        if (context.DistanceToPlayer < 2f)
            score -= 50;

        return Mathf.Max(score, 0);
    }
}
