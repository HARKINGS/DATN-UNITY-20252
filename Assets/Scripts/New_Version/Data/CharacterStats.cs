using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public int MaxHealth = 100;
    public int Damage = 10;

    public float MoveSpeed = 5f;

    public float AttackRange = 2f;

    public float KnockbackForce = 5f;
    public float KnockbackDuration = 0.2f;
}
