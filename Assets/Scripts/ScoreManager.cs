using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static event System.Action<int> OnDropCollected;

    [SerializeField] private TextMeshProUGUI counterText;

    private int count;
    private int score;
    private int currentCombo;

    public int Count => count;
    public int Score => score;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        // Подписываемся на статическое событие — Instance не нужен
        ComboTracker.OnComboChanged += OnComboChanged;
    }

    private void OnDisable()
    {
        ComboTracker.OnComboChanged -= OnComboChanged;
    }

    private void OnComboChanged(int combo)
    {
        currentCombo = combo;
    }

    public void AddDrop()
    {
        count++;

        // Бонус за комбо: комбо 1-2 = +0, 3-4 = +1, 5-6 = +2, 7-8 = +3
        int bonus = currentCombo >= 3 ? (currentCombo - 1) / 2 : 0;
        int points = 1 + bonus;

        score += points;
        counterText.text = score.ToString();
        OnDropCollected?.Invoke(score);
    }

    public void ResetScore()
    {
        count = 0;
        score = 0;
        currentCombo = 0;
        counterText.text = "0";
    }
}