using UnityEngine;

public class DownZoneAxe : MonoBehaviour
{
    ZoneTriggerManagerAxe manager;
    EnemyStateManagerAxe stateManager;

    private void OnTriggerEnter(Collider other)
    {
        manager = GetComponentInParent<ZoneTriggerManagerAxe>();
        stateManager = GetComponentInParent<EnemyStateManagerAxe>();
        if (manager == null)
        {
            Debug.Log("ZoneTriggerManager не найден в родительском объекте!");
        } 
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            Debug.Log("актив");
            manager.SetActiveZone("down");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.ResetZone("top");
            manager.defenceTime = 0;
        }
    }
}