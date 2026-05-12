using UnityEngine;
using UnityEngine.UI;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [field: SerializeField] public SkillEnum SkillType { get; private set; }

    [Header("Skill Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float coolDown;
    [SerializeField] protected float attackRange;

    protected DamageData currentDamageData;
    protected CharacterAnimation animator;
    protected SkillData skillData;
    protected float lastUseTime;

    protected virtual void Awake()
    {
        animator = GetComponent<CharacterAnimation>();
    }

    public virtual bool CanUse()
    {
        return Time.time >= lastUseTime + coolDown;
    }

    public virtual void Execute(DamageData damageData)
    {
        if (!CanUse()) return;

        currentDamageData = damageData;

        lastUseTime = Time.time;

        //animator.PlaySkill(this, animationTrigger);
    }

    public abstract void ApplyEffect();
}
