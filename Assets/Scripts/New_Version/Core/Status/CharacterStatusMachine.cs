using UnityEngine;

public class CharacterStatusMachine : MonoBehaviour
{
    private CharacterStatus currentStatus;
    private CharacterAnimation characterAnimation;

    void Start()
    {
        currentStatus = CharacterStatus.Idle;
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    private bool CheckChangeStatus(CharacterStatus newStatus)
    {
        if (currentStatus == CharacterStatus.Hurt || currentStatus == CharacterStatus.Stun)
        {
            return newStatus == CharacterStatus.Idle;
        }
        return true;
    }

    public void ChangeStatus(CharacterStatus newStatus)
    {
        if (currentStatus != newStatus && CheckChangeStatus(newStatus))
        {
            characterAnimation.ResetAnimation();
            currentStatus = newStatus;
        }
    }
}
