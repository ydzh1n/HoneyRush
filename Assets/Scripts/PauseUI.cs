using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance;

    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button menuButton;

    public bool IsOpen => pausePanel != null && pausePanel.activeSelf;

    private void Awake()
    {
        Instance = this;
        pauseButton.onClick.AddListener(Pause);
        menuButton.onClick.AddListener(() => GameRestart.ToMenu());
    }

    public void Pause()
    {
        if (!GameFlow.Started || GameFlow.GameOver) return;
        if (ControlPicker.Instance != null && ControlPicker.Instance.gameObject.activeSelf) return;
        if (BonusPicker.Instance != null && BonusPicker.Instance.gameObject.activeSelf) return;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart() => GameRestart.Run();

    public void OpenControls()
    {
        pausePanel.SetActive(false);
        if (ControlPicker.Instance != null) ControlPicker.Instance.Open();
    }
}