using System;
using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour, IHealth
{
    public AudioClip damagedClip;
    public Transform hitPoint;
    
    protected int CurrentHealth;
    protected CharacterStats stats;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath; // Giữ lại cho Animator nghe nếu cần
    public event Action OnHurt;

    public float GetHealthPercent() => (float)CurrentHealth / stats.MaxHealth;
    public int GetCurrentHealth() => CurrentHealth;

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStats>();
        CurrentHealth = stats.MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public virtual void ChangeHealth(DamageData damageData)
    {
        CurrentHealth -= damageData.Damage;

        if (CurrentHealth > stats.MaxHealth)
            CurrentHealth = stats.MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth > 0)
        {
            if (damagedClip != null && damageData.Damage > 0)
                CombatEvents.OnSoundRequested?.Invoke(damagedClip);
            OnHurt?.Invoke();
        }
        else
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
            HandleDeath(); // Gọi hàm xử lý cái chết đặc trưng của từng bên
        }
    }

    public virtual void Heal(DamageData damageData)
    {
        CurrentHealth += damageData.Damage;
        if (CurrentHealth > stats.MaxHealth)
            CurrentHealth = stats.MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth);
    }

    // Hàm trừu tượng buộc các lớp con phải tự định nghĩa hành vi khi chết
    protected abstract void HandleDeath();
}