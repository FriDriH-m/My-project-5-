using UnityEngine;

public class TopZone : MonoBehaviour
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
            manager.SetActiveZone("top");
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
