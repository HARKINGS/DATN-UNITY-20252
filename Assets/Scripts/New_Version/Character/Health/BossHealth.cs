using UnityEngine;

public class BossHealth : CharacterHealth
{
    protected override void HandleDeath()
    {
        Debug.Log("Boss đã bị hạ gục!");

        // Phát sự kiện toàn cục: Trận đấu kết thúc, Player Win = true
        CombatEvents.OnGameEnded?.Invoke(true);
    }
}