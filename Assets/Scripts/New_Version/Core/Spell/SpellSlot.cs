using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public Image iconImage;
    public GameObject highlight;

    public SkillBase AssignedSkill { get; private set; }

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor = Color.white;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = Vector3.one * 1.2f;

    public void SetSkill(SkillBase skill)
    {
        AssignedSkill = skill;

        if (skill != null)
        {
            iconImage.sprite = skill != null ? skill.skillIcon : null;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            AssignedSkill = null;
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
        }

        SetHighlight(false);
    }

    public void SetHighlight(bool value)
    {
        highlight.SetActive(value);

        iconImage.color = value ? highlightColor : normalColor;
        iconImage.rectTransform.localPosition = isActiveAndEnabled ? highlightScale : normalScale;
    }
}
