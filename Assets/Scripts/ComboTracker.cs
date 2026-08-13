using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    public static ComboTracker Instance;
    public static event System.Action<int> OnComboChanged;

    [SerializeField] private float comboTimeout = 2f;
    [SerializeField] private int maxCombo = 10;

    public int Combo { get; private set; }

    private float lastPickup;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (Combo > 0 && Time.time - lastPickup > comboTimeout)
            SetCombo(0);
    }

    public void RegisterPickup()
    {
        lastPickup = Time.time;
        SetCombo(Mathf.Min(Combo + 1, maxCombo));
    }

    public void Reset() => SetCombo(0);

    private void SetCombo(int value)
    {
        Combo = value;
        OnComboChanged?.Invoke(Combo);
    }
}