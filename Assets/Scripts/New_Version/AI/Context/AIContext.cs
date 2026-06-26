using UnityEngine;

public class AIContext
{
    public float DistanceToPlayer;

    public float BossHPPercent;

    public float PlayerAggression;

    public float PlayerAOEAgression;

    public float PlayerDefense;

    public float PlayerHealFrequency;

    public Transform Player;

    public float CombatTime;

    // Pattern flags
    public bool PlayerIsBursting;
    public bool PlayerIsKiting;
    public bool PlayerIsHitAndRun;

    public AIContext(
        float DistanceToPlayer, 
        float BossHPPercent, 
        float PlayerAggression, 
        float PlayerAOEAgression, 
        float PlayerDefense, 
        float PlayerHealFrequency, 
        Transform Player, 
        float CombatTime,
        bool PlayerIsBursting = false,
        bool PlayerIsKiting = false,
        bool PlayerIsHitAndRun = false)
    {
        this.DistanceToPlayer = DistanceToPlayer;
        this.BossHPPercent = BossHPPercent;
        this.PlayerAggression = PlayerAggression;
        this.PlayerAOEAgression = PlayerAOEAgression;
        this.PlayerDefense = PlayerDefense;
        this.PlayerHealFrequency = PlayerHealFrequency;
        this.Player = Player;
        this.CombatTime = CombatTime;
        this.PlayerIsBursting = PlayerIsBursting;
        this.PlayerIsKiting = PlayerIsKiting;
        this.PlayerIsHitAndRun = PlayerIsHitAndRun;
    }
}