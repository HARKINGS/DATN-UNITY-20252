using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SpellSlot : MonoBehaviour
{
    [Header("References")]
    public Image iconImage;
    public GameObject highlight;
    [SerializeField] private TMP_Text spellText;
    [SerializeField] private Image cooldownOverlay;

    public SkillBase AssignedSkill { get; private set; }

    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightColor = Color.white;
    private Vector3 normalScale = Vector3.one;
    private Vector3 highlightScale = Vector3.one * 1.2f;

    [Header("Pop Settings")]
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popDuration = 0.2f;

    public void SetSkill(SkillBase skill)
    {
        AssignedSkill = skill;

        if (skill != null)
        {
            cooldownOverlay.sprite = skill.skillIcon;
            iconImage.sprite = skill != null ? skill.skillIcon : null;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            AssignedSkill = null;
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
        }

        cooldownOverlay.fillAmount = 0; // Reset cooldown overlay
        SetHighlight(false);
    }

    public void SetHighlight(bool active)
    {
        highlight.SetActive(active);

        iconImage.color = active ? highlightColor : normalColor;
        iconImage.rectTransform.localPosition = isActiveAndEnabled ? highlightScale : normalScale;

        if (active && AssignedSkill != null)
            spellText.text = AssignedSkill.SkillType.ToString();

        spellText.enabled = active;
    }

    public void TriggerCooldown(float cooldownDuration)
    {
        if (cooldownOverlay != null)
            StartCoroutine(CooldownRoutine(cooldownDuration));
    }

    private IEnumerator CooldownRoutine(float cooldownDuration)
    {
        float elapsed = 0f;
        while (elapsed < cooldownDuration)
        {
            elapsed += Time.deltaTime;
            cooldownOverlay.fillAmount = Mathf.Clamp01(elapsed / cooldownDuration);
            yield return null;
        }
        cooldownOverlay.fillAmount = 0f; // Reset after cooldown
        yield return StartCoroutine(PopEffect());
    }

    private IEnumerator PopEffect()
    {
        float elapsed = 0f;
        float halfDuration = popDuration / 2f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            iconImage.rectTransform.localScale = Vector3.Lerp(normalScale, Vector3.one * popScale, t);
            yield return null;
        }

        elapsed = 0;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            iconImage.rectTransform.localScale = Vector3.Lerp(Vector3.one * popScale, normalScale, t);
            yield return null;
        }

        iconImage.rectTransform.localScale = normalScale; // Reset scale
    }
}
