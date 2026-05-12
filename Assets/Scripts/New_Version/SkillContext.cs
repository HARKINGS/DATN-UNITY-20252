using UnityEngine;

public class SkillContext : MonoBehaviour
{
    public CharacterAnimation Animator { get; private set; }

    public DamageData DamageData { get; private set; }

    public SkillContext(CharacterAnimation anim,  DamageData damageData)
    {
        Animator = anim;
        DamageData = damageData;
    }    
}
