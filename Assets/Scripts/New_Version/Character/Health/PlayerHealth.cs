using UnityEngine;

public class PlayerHealth : CharacterHealth
{
    protected override void HandleDeath()
    {
        Debug.Log("Player đã cạn máu!");

        // Phát sự kiện toàn cục: Trận đấu kết thúc, Player Win = false
        CombatEvents.OnGameEnded?.Invoke(false);
    }
}