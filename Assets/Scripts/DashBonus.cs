using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashBonus : MonoBehaviour
{
    [SerializeField] private float duration = 2.5f;
    [SerializeField] private Image dashIcon;

    private void OnEnable() => BonusActivator.OnBonusActivated += OnBonus;
    private void OnDisable() => BonusActivator.OnBonusActivated -= OnBonus;

    private void OnBonus(BonusType type)
    {
        if (type != BonusType.Dash) return;
        StopAllCoroutines();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        DashState.Start();
        if (AudioManager.Instance != null) AudioManager.Instance.Dash();
        if (dashIcon != null) dashIcon.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        DashState.Stop();
        if (dashIcon != null) dashIcon.gameObject.SetActive(false);
    }
}