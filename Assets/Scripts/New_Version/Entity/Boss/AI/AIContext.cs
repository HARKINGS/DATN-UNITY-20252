using UnityEngine;

public class AIContext
{
    public float DistanceToPlayer;

    public float BossHPPercent;

    public float PlayerAggression;

    public float PlayerDefense;

    public Transform Player;

    public AIContext(float DistanceToPlayer, float BossHPPercent, float PlayerAggression, float PlayerDefense, Transform Player)
    {
        this.DistanceToPlayer = DistanceToPlayer;
        this.BossHPPercent = BossHPPercent;
        this.PlayerAggression = PlayerAggression;
        this.PlayerDefense = PlayerDefense;
        this.Player = Player;
    }
}