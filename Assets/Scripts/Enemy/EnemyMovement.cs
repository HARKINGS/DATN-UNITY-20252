using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public float attackRange = 2;
    public float attackCooldown = 2;
    public float playerDectectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    private float attackCooldownTimer;
    private EnemyState enemyState;
    private int facingDirection = 1;

    private Rigidbody2D rb;
    private Transform player;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayer();
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        if (enemyState == EnemyState.Chasing)
            Chase();
        else if (enemyState == EnemyState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Chase()
    {
        if (
            (player.position.x > transform.position.x && facingDirection == -1)
            || (player.position.x < transform.position.x && facingDirection == 1)
        )
        {
            Flip();
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    private void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            detectionPoint.position,
            playerDectectRange,
            playerLayer
        );

        if (hits.Length > 0)
        {
            player = hits[0].transform;
            if (
                Vector2.Distance(transform.position, player.transform.position) <= attackRange
                && attackCooldownTimer <= 0
            )
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }
            else if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
                ChangeState(EnemyState.Chasing);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            ChangeState(EnemyState.Idle);
    }

    private void ChangeState(EnemyState newState)
    {
        // Exit the current state
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        // Update our current state
        enemyState = newState;

        // Update the new animation
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
}
