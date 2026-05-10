using UnityEngine;

public class CharacterAnimationBinder : MonoBehaviour
{
    [SerializeField]
    private CharacterHealth health;

    [SerializeField]
    private CharacterAnimation characterAnim;

    //private void OnEnable()
    //{
    //    health.OnHurt = characterAnim.SetBool("isHurt", true);
    //    health.OnDeath += OnDeath;
    //}

    //private void OnDisable()
    //{
    //    health.OnHurt -= characterAnim.PlayHurt;
    //    health.OnDeath -= OnDeath;
    //}

    //private void OnDeath()
    //{
    //    characterAnim.SetDead(true);
    //}
}