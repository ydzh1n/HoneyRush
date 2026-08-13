using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private GameObject hudRoot;
    [SerializeField] private TextMeshProUGUI bestText;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStart);

        if (GameFlow.FromRestart)
        {
            GameFlow.FromRestart = false;
            OnStart();
            return;
        }

        Time.timeScale = 0f;
        if (hudRoot != null) hudRoot.SetActive(false);
        if (bestText != null) bestText.text = $"Рекорд: {PlayerPrefs.GetInt("BestScore", 0)}";

        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        musicSlider.SetValueWithoutNotify(Mathf.Clamp01(PlayerPrefs.GetFloat("MusicVol", 0.5f)));
        sfxSlider.SetValueWithoutNotify(Mathf.Clamp01(PlayerPrefs.GetFloat("SfxVol", 0.8f)));

        musicSlider.onValueChanged.AddListener(v => { if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(v); });
        sfxSlider.onValueChanged.AddListener(v => { if (AudioManager.Instance != null) AudioManager.Instance.SetSfxVolume(v); });
    }

    private void OnStart()
    {
        gameObject.SetActive(false);
        if (hudRoot != null) hudRoot.SetActive(true);

        // Защита: если текущий режим недоступен на этой платформе — сбросить
        if (ControlSettings.HasChosen)
        {
            bool isMobile = Application.isMobilePlatform;
            if (ControlSettings.Current == ControlMode.Keyboard && isMobile)
            {
                ControlSettings.Current = ControlMode.Joystick;
            }
            else if (ControlSettings.Current == ControlMode.Gyro && !SystemInfo.supportsGyroscope)
            {
                ControlSettings.Current = isMobile ? ControlMode.Joystick : ControlMode.Keyboard;
            }
        }

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