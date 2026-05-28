using System;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [field: SerializeField] public SkillEnum SkillType { get; private set; }

    [Header("Skill Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask targetLayer;

    protected DamageData currentDamageData;
    protected CharacterAnimation animator;
    protected SkillData skillData;
    protected float lastUseTime;

    internal GameObject GetAttacker()
    {
        return currentDamageData.Attacker;
    }    

    protected virtual void Awake()
    {
        animator = GetComponent<CharacterAnimation>();
        animator.SetCurrentSkill(this);
        //Debug.Log(SkillType);
    }

    protected virtual void Start()
    {
        lastUseTime = -coolDown; // Cho phép sử dụng kỹ năng ngay khi bắt đầu
    }

    public virtual bool CanUse()
    {
        Debug.Log("Check " + SkillType + " at time " + Time.time);

        return (Time.time >= lastUseTime + coolDown && 
            GetComponent<CharacterStatusMachine>().CanCast);
    }

    public virtual void Execute(DamageData damageData)
    {
        if (!CanUse()) return;
        currentDamageData = damageData;
        lastUseTime = Time.time;
        CombatEvents.OnPlayerSkillUsed?.Invoke(this);
    }

    public abstract float Evaluate(AIContext context);

    public abstract void ApplyEffect();
}
