using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;

    [Header("Input")]
    public InputAction MoveAction;
    public InputAction AttackAction;
    public InputAction HealAction;
    public InputAction AOEAction;
    public InputAction DashAction;

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
        DashAction.Enable();
    }

    private void OnDisable()
    {
        MoveAction.Disable();
        AttackAction.Disable();
        AOEAction.Disable();
        HealAction.Disable();
        DashAction.Disable();
    }

    private void Update()
    {
        if (AttackAction.triggered)
        {
            Debug.Log("Attack Active");
            skillCaster.Execute(SkillEnum.Attack);
        }
        else if (HealAction.triggered)
            skillCaster.Execute(SkillEnum.Heal);
        else if (AOEAction.triggered)
            skillCaster.Execute(SkillEnum.AOE);
        else if (DashAction.triggered)
        {
            Debug.Log("Dash Active");
            skillCaster.Execute(SkillEnum.Dash);
        }
    }

    private void FixedUpdate()
    {
        movement.Move(MoveAction.ReadValue<Vector2>());
    }
}