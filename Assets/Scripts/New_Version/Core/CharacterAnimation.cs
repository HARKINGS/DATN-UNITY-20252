using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private CharacterHealth health;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        health.OnHurt += PlayHurt;
        health.OnDeath += PlayDeath;
    }

    private void OnDisable()
    {
        health.OnHurt -= PlayHurt;
        health.OnDeath -= PlayDeath;
    }

    private void PlayHurt()
    {
        animator.SetTrigger("Hurt");
    }

    private void PlayDeath()
    {
        animator.SetTrigger("Death");
    }

    public void SetMove(bool moving)
    {
        animator.SetBool("isMove", moving);
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }
}