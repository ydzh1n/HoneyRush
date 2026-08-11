using UnityEngine;

public class PickupTracker : MonoBehaviour
{
    private void OnEnable() => SpawnRegistry.RegisterPickup(transform.position);
    private void OnDisable() => SpawnRegistry.UnregisterPickup(transform.position);
}