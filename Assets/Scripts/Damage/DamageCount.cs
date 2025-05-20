using Unity.VisualScripting;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour _enemyStateManager; // для отключения после смерти
    [SerializeField] private MonoBehaviour _zoneTriggerManager;// для отключения после смерти
    [SerializeField] private string _tag;
    Transform[] allChildrens;
    private bool _hasDead = false;


    public float hitPoints = 100;
    private void Start()
    {
        allChildrens = transform.GetComponentsInChildren<Transform>();        
    }
    private void Update()
    {
        if (hitPoints <= 0 && !_hasDead)
        {
            foreach (Transform _object in allChildrens)
            {
                if (_object.CompareTag(_tag))
                {
                    Debug.Log("тег обнулен");
                    _object.tag = "Untagged";
                }
            }
            if (animator != null) animator.SetTrigger("Death");
            if (_enemyStateManager != null) { _enemyStateManager.enabled = false; }                
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = false; }
            _hasDead = true;
        }
    }
}
