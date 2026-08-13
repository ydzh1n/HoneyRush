using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip pickupClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip buzzReadyClip;
    [SerializeField] private AudioClip dashClip;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        musicSource.volume = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        sfxSource.volume = PlayerPrefs.GetFloat("SfxVol", 0.8f);
        musicSource.loop = true;
        musicSource.Play();
    }
    public void SetMusicVolume(float v)
    {
        musicSource.volume = v;
        PlayerPrefs.SetFloat("MusicVol", v);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float v)
    {
        sfxSource.volume = v;
        PlayerPrefs.SetFloat("SfxVol", v);
        PlayerPrefs.Save();
    }

    private void OnEnable() => BuzzMeter.OnBuzzReady += BuzzReady;
    private void OnDisable() => BuzzMeter.OnBuzzReady -= BuzzReady;

    public void Pickup() => sfxSource.PlayOneShot(pickupClip);
    public void Hit() => sfxSource.PlayOneShot(hitClip);
    public void BuzzReady() => sfxSource.PlayOneShot(buzzReadyClip);
    public void Dash() => sfxSource.PlayOneShot(dashClip);
}