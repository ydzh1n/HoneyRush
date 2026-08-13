using UnityEngine;
using UnityEngine.UI;

public class BonusPicker : MonoBehaviour
{
    public static BonusPicker Instance;

    [SerializeField] private Button magnetButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private Button dashButton;

    private void Awake()
    {
        Instance = this;
        magnetButton.onClick.AddListener(() => Pick(BonusType.Magnet));
        shieldButton.onClick.AddListener(() => Pick(BonusType.Shield));
        dashButton.onClick.AddListener(() => Pick(BonusType.Dash));
        BuzzMeter.OnBuzzReady += Open;
        gameObject.SetActive(false);
    }

    private void OnDestroy() => BuzzMeter.OnBuzzReady -= Open;

    private void Open()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Pick(BonusType type)
    {
        BonusActivator.Activate(type);
        if (BuzzMeter.Instance != null) BuzzMeter.Instance.Spend();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}