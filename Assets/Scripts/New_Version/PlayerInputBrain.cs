using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField]
    private CharacterMovement movement;

    [SerializeField]
    private CharacterCombat combat;

    private Vector2 move;

    [Header("Input")]
    public InputAction MoveAction;

    public InputAction AttackAction;

    private void Start()
    {
        MoveAction.Enable();
        AttackAction.Enable();
    }

    private void FixedUpdate()
    {
        move = MoveAction.ReadValue<Vector2>();
        if (move != Vector2.zero)
            Debug.Log(move);
        movement.Move(move);
    }
}