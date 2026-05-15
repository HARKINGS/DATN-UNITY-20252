using UnityEngine;

public class CharacterMovement : MonoBehaviour, IMovable
{
    private Rigidbody2D rb;
    private CharacterAnimation anim;

    private Vector2 moveDirection;
    private bool isFlipped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<CharacterAnimation>();
    }

    public void Move(Vector2 move)
    {
        moveDirection = move;
    }

    public void Stop()
    {
        moveDirection = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Vector2 position = (Vector2)rb.position + moveDirection * StatsManager.Instance.speed * Time.deltaTime;
        rb.MovePosition(position);
        anim.SetMove(moveDirection);
        Flip();
    }

    private void Flip()
    {
        isFlipped = 
            (moveDirection.x > 0 && transform.localScale.x < 0) ||
            (moveDirection.x < 0 && transform.localScale.x > 0);
           

        if (isFlipped)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}
