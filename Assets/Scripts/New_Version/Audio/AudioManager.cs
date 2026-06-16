using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void OnEnable()
    {
        CombatEvents.OnSoundRequested += PlaySFX;
    }
    private void OnDisable()
    {
        CombatEvents.OnSoundRequested -= PlaySFX;
    }

    // Phát nhạc nền (Music) - Lặp đi lặp lại
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}
