using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStart);

        if (GameFlow.FromRestart)
        {
            GameFlow.FromRestart = false;
            OnStart();
            return;
        }

        // морозим мир ПЕРВЫМ, чтобы никакая ошибка не оставила игру живой за меню
        Time.timeScale = 0f;

        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        musicSlider.SetValueWithoutNotify(Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVol", 0.5f)));
        sfxSlider.SetValueWithoutNotify(Mathf.Clamp01(PlayerPrefs.GetFloat("SfxVol", 0.8f)));

        // ленивое обращение: AudioManager может ещё не проснуться
        musicSlider.onValueChanged.AddListener(v => { if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(v); });
        sfxSlider.onValueChanged.AddListener(v => { if (AudioManager.Instance != null) AudioManager.Instance.SetSfxVolume(v); });
    }

    private void OnStart()
    {
        gameObject.SetActive(false);
        if (!ControlSettings.HasChosen && ControlPicker.Instance != null)
        {
            ControlPicker.Instance.Open();
        }
        else
        {
            GameFlow.StartGame();
            Time.timeScale = 1f;
        }
    }
}