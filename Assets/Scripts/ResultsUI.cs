using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject cocoon;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestText;
    [SerializeField] private Button restartButton;

    private void OnEnable() => GameFlow.OnGameEnded += Show;
    private void OnDisable() => GameFlow.OnGameEnded -= Show;

    private void Awake() => restartButton.onClick.AddListener(Restart);

    private void Show()
    {
        if (cocoon != null) cocoon.SetActive(true);

        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Count : 0;
        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("BestScore", best);
            PlayerPrefs.Save();
        }

        scoreText.text = $"Нектар: {score}";
        bestText.text = $"Рекорд: {best}";
        panel.SetActive(true);
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        GameFlow.ResetGame();
        MagnetState.Reset();
        ShieldState.Reset();
        DashState.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}