using System;
using UnityEngine;

public class CharacterStatusMachine : MonoBehaviour
{
    public CharacterStatus CurrentState { get; private set; }

    public event Action<CharacterStatus> OnStateChanged;

    public bool CanMove =>
        CurrentState == CharacterStatus.Idle ||
        CurrentState == CharacterStatus.Move ||
        CurrentState == CharacterStatus.Dash;

    public bool CanCast =>
        CurrentState == CharacterStatus.Idle ||
        CurrentState == CharacterStatus.Move;

    public bool CanAttack =>
        CurrentState == CharacterStatus.Idle ||
        CurrentState == CharacterStatus.Move;

    public void ChangeStatus(CharacterStatus newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        OnStateChanged?.Invoke(newState);
    }
}