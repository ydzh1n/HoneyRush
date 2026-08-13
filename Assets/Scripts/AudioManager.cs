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
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // музыка живёт между забегами
        musicSource.loop = true;
        musicSource.Play();
    }

    private void OnEnable() => BuzzMeter.OnBuzzReady += BuzzReady;
    private void OnDisable() => BuzzMeter.OnBuzzReady -= BuzzReady;

    public void Pickup() => sfxSource.PlayOneShot(pickupClip);
    public void Hit() => sfxSource.PlayOneShot(hitClip);
    public void BuzzReady() => sfxSource.PlayOneShot(buzzReadyClip);
    public void Dash() => sfxSource.PlayOneShot(dashClip);
}