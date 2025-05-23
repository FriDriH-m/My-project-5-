using Unity.VisualScripting;
using UnityEngine;

public class DamageCount : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour _enemyStateManager; // для отключения после смерти
    [SerializeField] private MonoBehaviour _zoneTriggerManager;// для отключения после смерти
    [SerializeField] private string _tag;
    [SerializeField] private PlayerDamage _playerDamage;
    Transform[] allChildrens;
    public float hitPoints;

    private Vector3 startCoordination;
    private Quaternion startRotation;
    public float startHitPoints;
    
    private void Start()
    {
        startHitPoints = hitPoints;
        startCoordination = transform.position;
        startRotation = transform.rotation;

        _playerDamage = FindFirstObjectByType<PlayerDamage>();
        if (_playerDamage == null) Debug.Log("PlayerDamage не найден");
        else Debug.Log("PlayerDamage найден");
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
        if (_playerDamage.hitPoints <= 0)
        {
            transform.position = startCoordination;
            transform.rotation = startRotation;
            hitPoints = startHitPoints;
        }
    }
}
