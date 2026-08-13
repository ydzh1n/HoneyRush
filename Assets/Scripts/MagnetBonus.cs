using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MagnetBonus : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private Image magnetIcon;

    private void OnEnable() => BonusActivator.OnBonusActivated += OnBonus;
    private void OnDisable() => BonusActivator.OnBonusActivated -= OnBonus;

    private void OnBonus(BonusType type)
    {
        if (type != BonusType.Magnet) return;
        StopAllCoroutines();
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        MagnetState.Start();
        if (magnetIcon != null) magnetIcon.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        MagnetState.Stop();
        if (magnetIcon != null) magnetIcon.gameObject.SetActive(false);
    }
}