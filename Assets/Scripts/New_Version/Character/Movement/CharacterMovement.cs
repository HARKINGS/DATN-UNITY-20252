using UnityEngine;

public class CharacterMovement : MonoBehaviour, IMovable
{
    private CharacterHealth health;
    private Rigidbody2D rb;
    private CharacterAnimation anim;

    private Vector2 moveDirection;
    private bool isFlipped = false;

    public CharacterAnimation GetAnimation()
    {
        return this.anim; 
    }   
    
    public CharacterHealth GetHealth()
    {
        return this.health;
    }

    private void Awake()
    {
        //MoveAction.Enable();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<CharacterAnimation>();
        health = GetComponent<CharacterHealth>();
    }

    public void Move(Vector2 moveDirection)
    {
        this.moveDirection = moveDirection;
    }

    public void Stop()
    {
        moveDirection = Vector2.zero;
    }

    public Vector2 getMove()
    {
        return moveDirection;
    }    

    private void FixedUpdate()
    {
        Vector2 position = (Vector2)rb.position + moveDirection * GetComponent<CharacterStats>().MoveSpeed * Time.deltaTime;
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
