using System.Collections.Generic;
using UnityEngine;

public class SpellUIManager : MonoBehaviour
{
    [SerializeField] private List<SpellSlot> spellSlots = new List<SpellSlot>();

    public void ShowSkills(List<SkillBase> skills)
    {
        for (int i = 0; i < spellSlots.Count; i++)
        {
            if (i < skills.Count)
            {
                Debug.Log($"Assigning skill {skills[i].SkillType} to slot {i}");
                spellSlots[i].SetSkill(skills[i]);
                spellSlots[i].gameObject.SetActive(true);
            }
            else
            {
                spellSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void HighlightSkill(SkillBase activeSkill)
    {
        foreach (var slot in spellSlots)
        {
            if (slot.AssignedSkill == activeSkill)
            {
                slot.SetHighlight(true);
            }
            else
            {
                slot.SetHighlight(false);
            }
        }
    }
}
