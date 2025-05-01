using UnityEngine;

public class DownZoneShield : MonoBehaviour
{
    ZoneTriggerManager manager;
    EnemyStateManager stateManager;

    private void OnTriggerEnter(Collider other)
    {
        manager = GetComponentInParent<ZoneTriggerManager>();
        stateManager = GetComponentInParent<EnemyStateManager>();
        if (manager == null)
        {
            Debug.Log("ZoneTriggerManager не найден в родительском объекте!");
        }
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.SetActiveZone("down");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.ResetZone("down");
            manager.defenceTime = 0;
        }
    }
}
