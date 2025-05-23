using System.Collections;
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
    private bool alreadyDead = false;

    private Vector3 startCoordination;
    private Quaternion startRotation;
    public float startHitPoints;
    
    private void Start()
    {
        startHitPoints = hitPoints;
        startCoordination = transform.position;
        startRotation = transform.rotation;

        _playerDamage = FindFirstObjectByType<PlayerDamage>();
        allChildrens = transform.GetComponentsInChildren<Transform>();        
    }
    private void Update()
    {
        if (hitPoints <= 0 && !alreadyDead )
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
            alreadyDead = true;
        }
        if (_playerDamage.hitPoints <= 0 && alreadyDead)
        {
            if (_enemyStateManager != null) { _enemyStateManager.enabled = true; }
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = true; }

            animator.Rebind();
            animator.Update(0f);
            animator.Play("Idle", 0);

            transform.position = startCoordination + new Vector3(0, 0, 1);
            transform.rotation = startRotation;
            hitPoints = startHitPoints;

            alreadyDead = false;
        }
    }
}
