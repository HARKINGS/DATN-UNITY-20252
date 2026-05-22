using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBrain : MonoBehaviour
{
    [SerializeField] private SkillCaster skillCaster;
    [SerializeField] private CharacterMovement movement;

    private CharacterStatusMachine StatusMachine;

    [Header("Input")]
    public InputAction MoveAction;
    public InputAction AttackAction;
    public InputAction HealAction;
    public InputAction AOEAction;
    public InputAction DashAction;

    private void Start()
    {
        StatusMachine = GetComponent<CharacterStatusMachine>();
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
        if (!StatusMachine.CanAttack || !StatusMachine.CanCast || !StatusMachine.CanMove)
            return;

        //Debug.Log("Current Status is: " + StatusMachine.CurrentState);

        if (AttackAction.triggered)
        {
            skillCaster.Execute(SkillEnum.Attack);
        }
        else if (DashAction.triggered)
        {
            skillCaster.Execute(SkillEnum.Dash);
        }
        else
        {
            if (HealAction.triggered)
                skillCaster.Execute(SkillEnum.Heal);
            else if (AOEAction.triggered)
                skillCaster.Execute(SkillEnum.AOE); 
        }  
    }

    private void FixedUpdate()
    {
        movement.Move(MoveAction.ReadValue<Vector2>());
    }
}