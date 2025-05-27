using UnityEngine;

public class TopZoneAxe : MonoBehaviour
{
    ZoneTriggerManagerAxe manager;
    EnemyStateManagerAxe stateManager;

    private void OnTriggerEnter(Collider other)
    {
        stateManager = GetComponentInParent<EnemyStateManagerAxe>();
        manager = GetComponentInParent<ZoneTriggerManagerAxe>();
        if (manager == null)
        {
            Debug.Log("ZoneTriggerManager не найден в родительском объекте!");
        }
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.defenseSide = "top";
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.defenseSide = "";
            manager.defenceTime = 0;
        }
    }
}
