using System;
using System.Collections;
using UnityEngine;

public class CharacterHealth : MonoBehaviour, IHealth
{
    public Transform hitPoint;
    private int CurrentHealth;
    private CharacterStats stats;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    public event Action OnHurt;

    public float GetHealthPercent()
    {
        return (float)CurrentHealth / stats.MaxHealth;
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }    

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        CurrentHealth = stats.MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void ChangeHealth(DamageData damageData)
    {
        //Debug.Log("Change Health: " + damageData.Damage);
        CurrentHealth -= damageData.Damage;
        //Debug.Log(CurrentHealth);

        if (CurrentHealth > stats.MaxHealth)
            CurrentHealth = stats.MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth > 0)
        {
            OnHurt?.Invoke();
        }
        else
        {
            CurrentHealth = 0;
            OnDeath?.Invoke();
        }
    }

    public void Heal(DamageData damageData)
    {
        //Debug.Log("Heal Health: " + damageData.Damage);
        CurrentHealth += damageData.Damage;
        //Debug.Log(CurrentHealth);

        if (CurrentHealth > stats.MaxHealth)
            CurrentHealth = stats.MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth);
    }    
}
