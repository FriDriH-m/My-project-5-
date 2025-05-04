using Unity.VisualScripting;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private MonoBehaviour _enemyStateManager; // для отключения после смерти
    [SerializeField] private MonoBehaviour _zoneTriggerManager;// для отключения после смерти
    public float hitPoints = 100;
    private void Update()
    {
        if (hitPoints <= 0)
        {
            animator.SetTrigger("Death");
            if (_enemyStateManager != null && _zoneTriggerManager != null)
            {
                _enemyStateManager.enabled = false;
                _zoneTriggerManager.enabled = false;
            }
        }
    }
}
