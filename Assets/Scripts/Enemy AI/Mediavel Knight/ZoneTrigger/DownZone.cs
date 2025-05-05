using UnityEngine;

public class DownZone : MonoBehaviour
{
    ZoneTriggerManager manager;
    EnemyStateManager stateManager;

    private void OnTriggerEnter(Collider other)
    {
        stateManager = GetComponentInParent<EnemyStateManager>();
        manager = GetComponentInParent<ZoneTriggerManager>();
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.defenseSide = "down";
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}