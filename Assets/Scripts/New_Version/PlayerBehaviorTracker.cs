using UnityEngine;

public class PlayerBehaviorTracker : MonoBehaviour
{
    public int AttackCount { get; private set; }
    public int DodgeCount { get; private set; }

    public float AggressionLevel =>
        AttackCount / Mathf.Max(1f, CombatTime);

    public float DodgeRate =>
        DodgeCount / Mathf.Max(1f, CombatTime);

    public float CombatTime { get; private set; }

    private void Update()
    {
        CombatTime += Time.deltaTime;
    }

    public void RegisterAttack()
    {
        AttackCount++;
    }

    public void RegisterDodge()
    {
        AttackCount++;
    }
}
