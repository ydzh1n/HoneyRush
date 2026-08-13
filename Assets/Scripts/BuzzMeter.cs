using UnityEngine;
using UnityEngine.UI;

public class BuzzMeter : MonoBehaviour
{
    public static BuzzMeter Instance;
    public static event System.Action OnBuzzReady;
    public static event System.Action OnBuzzSpent;

    [SerializeField] private Image fill;
    [SerializeField] private int comboPerBuzz = 10;

    public float Progress { get; private set; }
    public bool IsReady => Progress >= 1f;

    private void Awake() => Instance = this;

    private void OnEnable() => ComboTracker.OnComboChanged += ApplyCombo;
    private void OnDisable() => ComboTracker.OnComboChanged -= ApplyCombo;

    private void ApplyCombo(int combo)
    {
        float prev = Progress;
        Progress = Mathf.Clamp01((float)combo / comboPerBuzz);
        fill.fillAmount = Progress;

        if (IsReady && prev < 1f) OnBuzzReady?.Invoke();
    }

    public void Spend()
    {
        Progress = 0f;
        fill.fillAmount = 0f;
        if (ComboTracker.Instance != null) ComboTracker.Instance.Reset();
        OnBuzzSpent?.Invoke();
    }
}