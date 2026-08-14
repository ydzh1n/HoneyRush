using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    public static DifficultyScaler Instance;

    [Header("Рост сложности")]
    [Tooltip("Какие секунды игры считаются одним 'уровнем' сложности")]
    [SerializeField] private float timePerLevel = 15f;

    [Tooltip("На сколько увеличивается сложность за каждый уровень (1.0 = +0%)")]
    [SerializeField] private float multiplierStep = 0.15f;

    [Tooltip("Максимальный предел сложности (чтобы игра не стала невозможной)")]
    [SerializeField] private float maxMultiplier = 2.0f;

    // Текущий множитель (1.0 в начале, максимум 2.0)
    public float Multiplier { get; private set; } = 1f;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Считаем время только во время активного забега
        if (!GameFlow.Started || GameFlow.GameOver) return;

        timer += Time.deltaTime;

        if (timer >= timePerLevel)
        {
            timer = 0f;
            // Увеличиваем множитель, но не выше максимума
            Multiplier = Mathf.Min(Multiplier + multiplierStep, maxMultiplier);
            Debug.Log($"Сложность возросла! Множитель: {Multiplier:F1}x");
        }
    }

    // Сброс при новом забеге
    public void ResetDifficulty()
    {
        Multiplier = 1f;
        timer = 0f;
    }
}