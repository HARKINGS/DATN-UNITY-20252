using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private CharacterHealth health;
    private SkillBase currentSkill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        health.OnHurt += PlayHurt;
        health.OnDeath += PlayDead;
    }

    private void OnDisable()
    {
        health.OnHurt -= PlayHurt;
        health.OnDeath -= PlayDead;
    }

    private void PlayHurt()
    {
        StartCoroutine(HurtEffectRoutine(0.25f));
    }

    IEnumerator HurtEffectRoutine(float hurtTime)
    {
        var statusMachine = GetComponent<CharacterStatusMachine>();

        if(statusMachine != null)
            statusMachine.ChangeStatus(CharacterStatus.Hurt);

        animator.SetBool("isCasting", false);
        animator.SetBool("isAttack", false);
        AnimationEvent_EndSkill();

        GetComponent<CharacterMovement>().enabled = false;
        animator.SetBool("isHurt", true);
        
        yield return new WaitForSeconds(hurtTime);

        animator.SetBool("isHurt", false);
        GetComponent<CharacterMovement>().enabled = true;

        // 3. TRẢ TRẠNG THÁI LINH HOẠT: Sau khi hết đau, check xem có đang di chuyển không để về Move hoặc Idle
        if (statusMachine != null && statusMachine.CurrentState == CharacterStatus.Hurt)
        {
            statusMachine.ChangeStatus(CharacterStatus.Idle);
        }
    }

    private void PlayDead()
    {
        StartCoroutine(DeathRoutine(1.5f));
    }

    IEnumerator DeathRoutine(float deadTime)
    {
        animator.SetBool("isDead", true);

        // Vô hiệu hóa các script điều khiển và va chạm để tránh lỗi logic khi đang chết
        GetComponent<CharacterMovement>().enabled = false;

        // Đợi thời gian anim chết chạy (ví dụ 2 giây)
        yield return new WaitForSeconds(deadTime);
        Destroy(gameObject);
    }

    public bool CheckStatus(string Flag)
    {
        return animator.GetBool(Flag);
    }

    public bool CheckCast()
    {
        return animator.GetBool("isCasting");
    }

    public void PlaySkill(SkillBase skill, string flag)
    {
        SetCurrentSkill(skill);
        animator.SetBool(flag, true);
    }

    public void SetCurrentSkill(SkillBase skill)
    {
        currentSkill = skill;
    }

    // Animation Event
    public void AnimationEvent_ApplySkill()
    {
        //Debug.Log("Apply_Effect");
        currentSkill?.ApplyEffect();
    }

    // Animation Event
    public void AnimationEvent_EndSkill()
    {
        currentSkill = null;
    }

    public void SetMove(Vector2 move)
    {
        animator.SetFloat("horizontal", Mathf.Abs(move.x));
        animator.SetFloat("vertical", Mathf.Abs(move.y));

        if (move.x == 0 && move.y == 0)
        {
            if(GetComponent<CharacterStatusMachine>().CanCast)
                GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Idle);
            animator.SetBool("isIdle", true);
        }
        else
        {
            GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Move);
            animator.SetBool("isIdle", false);
        }
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttack", false);
        if(animator.GetBool("isIdle"))
        {
            GetComponent<CharacterStatusMachine>().ChangeStatus(CharacterStatus.Idle);
        }
    }

    public void FinishCast()
    {
        animator.SetBool("isCasting", false);
    }

    public void ResetAnimation()
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(param.name, false);
            if (param.type == AnimatorControllerParameterType.Float)
                animator.SetFloat(param.name, 0);
        }
    }

    public void InterruptCurrentSkill()
    {
        currentSkill = null;

        animator.SetBool("isCasting", false);
        animator.SetBool("isAttack", false);
    }
}