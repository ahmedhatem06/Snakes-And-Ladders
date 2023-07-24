using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource soundEffects_AS;
    [Header("Sound Effects Clips")]
    public AudioClip pitfallAudioClip;
    public AudioClip shortcutAudioClip;
    public AudioClip victoryAudioClip;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        soundEffects_AS = GetComponent<AudioSource>();
    }

    public void PitfallSoundEffect()
    {
        soundEffects_AS.clip = pitfallAudioClip;
        soundEffects_AS.Play();
    }

    public void ShortcutSoundEffect()
    {
        soundEffects_AS.clip = shortcutAudioClip;
        soundEffects_AS.Play();
    }

    public void VictorySoundEffect()
    {
        soundEffects_AS.clip = victoryAudioClip;
        soundEffects_AS.Play();
    }
}
