using UnityEngine;

public class AIContext
{
    public float DistanceToPlayer;

    public float BossHPPercent;

    public float PlayerAggression;

    public float PlayerAOEAgression;

    public float PlayerDefense;

    public Transform Player;

    public AIContext(float DistanceToPlayer, float BossHPPercent, float PlayerAggression, float PlayerAOEAgression, float PlayerDefense, Transform Player)
    {
        this.DistanceToPlayer = DistanceToPlayer;
        this.BossHPPercent = BossHPPercent;
        this.PlayerAggression = PlayerAggression;
        this.PlayerAOEAgression = PlayerAOEAgression;
        this.PlayerDefense = PlayerDefense;
        this.Player = Player;
    }
}