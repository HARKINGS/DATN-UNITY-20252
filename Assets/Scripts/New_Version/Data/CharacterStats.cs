using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    public int MaxHealth = 100;
    public float MoveSpeed = 5f;

    //public int Damage = 10;
    //public float AttackRange = 2f;

    public float KnockbackTime = 2f;
    public float KnockbackForce = 5f;
    public float KnockbackDuration = 0.2f;
}
