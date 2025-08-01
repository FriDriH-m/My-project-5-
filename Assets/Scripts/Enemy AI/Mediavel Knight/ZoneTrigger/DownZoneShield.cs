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
            Debug.Log("ZoneTriggerManager �� ������ � ������������ �������!");
        }
        if (other.gameObject.CompareTag("Weapon") && !stateManager.isAttacking)
        {
            manager.defenseSide = "down";
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
