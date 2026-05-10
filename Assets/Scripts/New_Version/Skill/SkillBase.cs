using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [SerializeField] protected float coolDown;

    protected float lastUseTime;

    public virtual void Execute(DamageData damageData)
    {
        lastUseTime = Time.time;
    }
}
