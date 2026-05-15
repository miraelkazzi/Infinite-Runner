using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip gameLoseSound;

    [Header("Volumes")]
    [Range(0f, 1f)][SerializeField] private float backgroundMusicVolume = 0.5f;
    [Range(0f, 2f)][SerializeField] private float buttonClickVolume = 0.8f;
    [Range(0f, 1f)][SerializeField] private float coinPickupVolume = 0.8f;
    [Range(0f, 1f)][SerializeField] private float gameLoseVolume = 0.8f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;

        PlayBackgroundMusic();
    }

    void Update()
    {
        if (musicSource != null)
            musicSource.volume = backgroundMusicVolume;
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.volume = backgroundMusicVolume;
        musicSource.Play();
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClickSound, buttonClickVolume);
    }

    public void PlayCoinPickup()
    {
        PlaySound(coinPickupSound, coinPickupVolume);
    }

    public void PlayGameLose()
    {
        PlaySound(gameLoseSound, gameLoseVolume);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }
}