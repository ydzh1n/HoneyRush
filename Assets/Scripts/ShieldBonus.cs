using UnityEngine;
using UnityEngine.UI;

public class ShieldBonus : MonoBehaviour
{
    [SerializeField] private Image shieldIcon;

    private void OnEnable()
    {
        BonusActivator.OnBonusActivated += OnBonus;
        ShieldState.OnChanged += SetIcon;
    }

    private void OnDisable()
    {
        BonusActivator.OnBonusActivated -= OnBonus;
        ShieldState.OnChanged -= SetIcon;
    }

    private void OnBonus(BonusType type)
    {
        if (type == BonusType.Shield)
            ShieldState.Gain();
    }

    private void SetIcon(bool on)
    {
        if (shieldIcon != null) shieldIcon.gameObject.SetActive(on);
    }
}