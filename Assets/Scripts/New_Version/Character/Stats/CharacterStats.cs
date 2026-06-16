using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;

    [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;

    [field: SerializeField] public float EffectTime { get; private set; } = 2f;

    [field: SerializeField] public float EffectForce { get; private set; } = 5f;

    [field: SerializeField] public float EffectDuration { get; private set; } = 0.2f;
}