using System;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;
    [SerializeField] private Rigidbody2D rb;

    private bool facingRight = true;

    private void Start()
    {
        visualRoot = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleFlip();
    }

    private void HandleFlip()
    {
        Debug.Log("Flip111!");
        if (rb.linearVelocity.x == 0)
            return;

        bool movingRight = rb.linearVelocity.x > 0;

        if (movingRight != facingRight)
        {
            Debug.Log("Flip!");
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = visualRoot.localScale;
        scale.x *= -1;

        visualRoot.localScale = scale;
    }
}