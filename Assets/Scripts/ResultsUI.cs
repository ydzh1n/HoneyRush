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
    [SerializeField] private Button menuButton;

    private void OnEnable() => GameFlow.OnGameEnded += Show;
    private void OnDisable() => GameFlow.OnGameEnded -= Show;

    private void Awake()
    {
        restartButton.onClick.AddListener(Restart);
        menuButton.onClick.AddListener(() => GameRestart.ToMenu());
    }

    private void Show()
    {
        if (cocoon != null) cocoon.SetActive(true);

        int score = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
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

    private void Restart() => GameRestart.Run();
}