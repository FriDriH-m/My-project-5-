using UnityEngine;

public class LeftZone : MonoBehaviour
{
    ZoneTriggerManager manager;
    EnemyStateManager stateManager;

    private void OnTriggerEnter(Collider other)
    {
        stateManager = GetComponentInParent<EnemyStateManager>();
        manager = GetComponentInParent<ZoneTriggerManager>();
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.defenseSide = "left";
        }       
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {            
            manager.defenseSide = "";
        }
    }
}