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

    public void PlaySkill(
        SkillBase skill,
        string trigger)
    {
        currentSkill = skill;

        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
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

    public void PlayAttack()
    {
        animator.SetBool("isAttack", true);
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttack", false);
    }    
}