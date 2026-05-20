using UnityEngine;

public class BossMemory : MonoBehaviour
{
    public int PlayerAttackCount
    { get; private set; }

    public int PlayerDashCount
    { get; private set; }

    public int PlayerHealCount
    { get; private set; }

    public int PlayerAOECount
    { get; private set; }

    public float CombatTime
    { get; private set; }

    private void Update()
    {
        CombatTime += Time.deltaTime;
    }

    public void RegisterAttack()
    {
        PlayerAttackCount++;
    }

    public void RegisterDash()
    {
        PlayerDashCount++;
    }

    public void RegisterHeal()
    {
        PlayerHealCount++;
    }

    public void RegisterAOE()
    {
        PlayerAOECount++;
    }

    public float GetAggressionLevel()
    {
        return PlayerAttackCount
            / Mathf.Max(CombatTime, 1);
    }

    public float GetDefensiveLevel()
    {
        return PlayerDashCount
            / Mathf.Max(CombatTime, 1);
    }
}