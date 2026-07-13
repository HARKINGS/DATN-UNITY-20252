using System;
using TMPro;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [field: SerializeField] public SkillEnum SkillType { get; private set; }

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip castSound;

    [Header("Skill Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask targetLayer;
    [field: SerializeField] public char KeySkill { get; protected set; }

    [field: SerializeField] public Sprite skillIcon { get; private set; }

    protected DamageData currentDamageData;
    protected CharacterAnimation animator;
    protected SkillData skillData;
    protected float lastUseTime;

    internal GameObject GetAttacker()
    {
        return currentDamageData.Attacker;
    }  
    
    public float GetCooldown()
    {
        return coolDown;
    }

    public void ResetCooldown()
    {
        lastUseTime = Time.time - coolDown; // Cho phép dùng ngay
    }

    protected virtual void Awake()
    {
        animator = GetComponent<CharacterAnimation>();
        animator.SetCurrentSkill(this);
    }

    protected virtual void Start()
    {
        lastUseTime = -coolDown; // Cho phép sử dụng kỹ năng ngay khi bắt đầu
    }

    public virtual bool CanUse()
    {
        return (Time.time >= lastUseTime + coolDown && 
            GetComponent<CharacterStatusMachine>().CanCast);
    }

    public virtual void Execute(DamageData damageData)
    {
        if (!CanUse()) return;
        currentDamageData = damageData;
        lastUseTime = Time.time;

        if (castSound != null)
            CombatEvents.OnSoundRequested?.Invoke(castSound);
        CombatEvents.OnPlayerSkillUsed?.Invoke(this);
    }

    public abstract float Evaluate(AIContext context);

    public abstract void ApplyEffect();
}
