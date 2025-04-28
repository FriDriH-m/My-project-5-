using UnityEngine;

public class RightZone : MonoBehaviour
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
            manager.SetActiveZone("right");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && manager != null)
        {
            manager.ResetZone("right");
            manager.defenceTime = 0;
        }
    }
}