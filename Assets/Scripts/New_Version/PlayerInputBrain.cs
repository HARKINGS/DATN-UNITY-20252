using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;

    //private CharacterState characterState;

    [Header("Input")]
    public InputAction MoveAction;
    public InputAction AttackAction;
    public InputAction HealAction;
    public InputAction AOEAction;

    private void Start()
    {
        skillCaster = GetComponent<SkillCaster>();
        movement = GetComponent<CharacterMovement>();
    }

    private void OnEnable()
    {
        MoveAction.Enable();
        AttackAction.Enable();
        AOEAction.Enable();
        HealAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
        AttackAction.Disable();
        AOEAction.Disable();
        HealAction.Disable();
    }

    private void Update()
    {
        if (AttackAction.triggered)
            skillCaster.Execute(SkillEnum.Attack);
        else if (HealAction.triggered)
            skillCaster.Execute(SkillEnum.Heal);
        else if (AOEAction.triggered)
            skillCaster.Execute(SkillEnum.AOE);
    }

    private void FixedUpdate()
    {
        movement.Move(MoveAction.ReadValue<Vector2>());
    }
}