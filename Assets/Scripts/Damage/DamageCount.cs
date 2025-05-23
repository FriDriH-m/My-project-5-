using Unity.VisualScripting;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour _enemyStateManager; // для отключения после смерти
    [SerializeField] private MonoBehaviour _zoneTriggerManager;// для отключения после смерти
    [SerializeField] private string _tag;
    Transform[] allChildrens;


    public float hitPoints = 100;
    private void Start()
    {
        allChildrens = transform.GetComponentsInChildren<Transform>();        
    }
    private void Update()
    {
        if (hitPoints <= 0)
        {
            foreach (Transform _object in allChildrens)
            {
                if (_object.CompareTag(_tag))
                {
                    _object.tag = "Untagged";
                }
            }
            if (animator != null) animator.SetTrigger("Death");
           
            if (_enemyStateManager != null) { _enemyStateManager.enabled = false; }                
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = false; }
            this.enabled = false;
        }
    }
}
