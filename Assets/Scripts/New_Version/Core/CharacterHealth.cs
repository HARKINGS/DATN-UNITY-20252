using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IHealth
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private int CurrentHealth;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnHurt;

    private void Awake()
    {
        CurrentHealth = stats.MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void ChangeHealth(DamageData damageData)
    {
        CurrentHealth += damageData.Damage;

        OnHealthChanged?.Invoke(CurrentHealth);
        OnHurt?.Invoke();

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
