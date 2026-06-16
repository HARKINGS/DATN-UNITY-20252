public interface ISkill
{
    bool CanUse();
    void Execute(DamageData damageData);
    //void Execute(SkillContext context);
}