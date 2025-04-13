using UnityEngine;

public class MiddleZone : MonoBehaviour
{
    ZoneTriggerManager manager;
    private void Start()
    {
        manager = FindFirstObjectByType<ZoneTriggerManager>();

        if (manager == null)
        {
            Debug.LogError("ZoneTriggerManager не найден в сцене!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.SetActiveZone("middle");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.ResetZone("middle");
        }
    }
}