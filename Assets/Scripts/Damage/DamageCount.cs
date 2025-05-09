using Unity.VisualScripting;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour _enemyStateManager; // для отключения после смерти
    [SerializeField] private MonoBehaviour _zoneTriggerManager;// для отключения после смерти
    public float hitPoints = 100;
    private void Update()
    {
        if (hitPoints <= 0)
        {
            animator.SetTrigger("Death");
            if (_enemyStateManager != null) { _enemyStateManager.enabled = false; }                
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = false; }
        }
    }
}
