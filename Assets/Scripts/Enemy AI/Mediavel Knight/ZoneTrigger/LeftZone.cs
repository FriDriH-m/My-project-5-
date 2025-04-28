using UnityEngine;

public class LeftZone : MonoBehaviour
{
    ZoneTriggerManager manager;
    EnemyStateManager stateManager;

    private void OnTriggerEnter(Collider other)
    {
        stateManager = GetComponentInParent<EnemyStateManager>();
        manager = GetComponentInParent<ZoneTriggerManager>();
        if (manager == null)
        {
            Debug.Log("ZoneTriggerManager не найден в родительском объекте!");
        }
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.SetActiveZone("left");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.ResetZone("left");
            manager.defenceTime = 0;
        }
    }
}