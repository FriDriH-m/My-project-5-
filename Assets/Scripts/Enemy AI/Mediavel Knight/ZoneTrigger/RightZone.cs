using UnityEngine;

public class RightZone : MonoBehaviour
{
    ZoneTriggerManager manager;
    EnemyStateManager stateManager;

    private void OnTriggerEnter(Collider other)
    {
        stateManager = GetComponentInParent<EnemyStateManager>();
        manager = GetComponentInParent<ZoneTriggerManager>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.defenseSide = "right";
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