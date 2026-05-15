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
        GetComponent<CharacterMovement>().enabled = false;
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(hurtTime);
        animator.SetBool("isHurt", false);
        GetComponent<CharacterMovement>().enabled = true;
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

        // gameObject.SetActive(false);
        // Hoặc
        Destroy(gameObject);
    }

    public void PlaySkill(SkillBase skill, string trigger)
    {
        currentSkill = skill;
        Debug.Log(trigger);

        animator.SetBool(trigger, true);
    }

    public void SetCurrentSkill(SkillBase skill)
    {
        currentSkill = skill;
    }

    // Animation Event
    public void AnimationEvent_ApplySkill()
    {
        Debug.Log("Apply_Effect");
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
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttack", false);
    }

    public void FinishCast()
    {
        animator.SetBool("isCasting", false);
    }
}