using UnityEngine;

public class HazardTracker : MonoBehaviour
{
    private void OnEnable() => SpawnRegistry.RegisterHazard(transform.position);
    private void OnDisable() => SpawnRegistry.UnregisterHazard(transform.position);
}