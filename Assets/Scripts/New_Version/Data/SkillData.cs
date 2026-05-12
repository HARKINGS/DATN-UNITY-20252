using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill Data")]
public class SkillData : ScriptableObject
{
    public string skillName;

    public float cooldown;

    public int damage;

    public float range;

    public AnimationClip animationClip;
}
