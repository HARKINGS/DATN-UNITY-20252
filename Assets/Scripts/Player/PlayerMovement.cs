using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private float horizontal;
    private float vertical;

    public InputAction MoveAction;
    public InputAction AttackAction;
    public Animator animator;
    public PlayerCombat playerCombat;
    private Rigidbody2D rb2d;
    private Vector2 move;

    // true: di chuyển cùng hướng, false: di chuyển ngược hướng
    private bool isFacingDirect = true;

    private bool isAttack = false;
    private bool isKnockedBack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveAction.Enable();
        AttackAction.Enable();

        rb2d = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (AttackAction.triggered)
        {
            isAttack = true;
            playerCombat.Attack();
            CancelInvoke(nameof(EndAttack));
            Invoke(nameof(EndAttack), 0.5f);
        }

        // if (AttackAction.triggered && !isAttack)
        // {
        //     Debug.Log("Attack!");
        //     isAttack = true;
        //     CancelInvoke(nameof(EndAttack));
        //     Invoke(nameof(EndAttack), 0.5f);
        // }
    }

    private void EndAttack() => isAttack = false;

    private void FixedUpdate()
    {
        if (!isKnockedBack)
        {
            animator.SetFloat("horizontal", Mathf.Abs(move.x));
            animator.SetFloat("vertical", Mathf.Abs(move.y));
            // animator.SetBool("isAttack", isAttack);

            Flip();

            Vector2 position = (Vector2)rb2d.position + move * speed * Time.deltaTime;
            rb2d.MovePosition(position);
        }
    }

    private void Flip()
    {
        if (isAttack)
            return;

        if (
            (move.x > 0 && transform.localScale.x < 0) || (move.x < 0 && transform.localScale.x > 0)
        )
            isFacingDirect = false;
        else
            isFacingDirect = true;

        if (!isFacingDirect)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockedBack = true;
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb2d.linearVelocity = direction * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb2d.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}
