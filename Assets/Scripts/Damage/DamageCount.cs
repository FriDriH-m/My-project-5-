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
    [SerializeField] private float timeNoAttaking;
    [SerializeField] private Transform _golemHand1;
    [SerializeField] private Transform _golemHand2;
    Transform[] allChildrens;
    public float hitPoints;
    private bool alreadyDead = false;
    public bool attacking = false;
    public bool revive = false;

    private Vector3 startCoordination;
    private Quaternion startRotation;
    public float startHitPoints;
    
    private void Start()
    {
        startHitPoints = hitPoints;
        startCoordination = transform.position;
        startRotation = transform.rotation;
        animator = transform.GetComponent<Animator>();
        _playerDamage = FindFirstObjectByType<PlayerDamage>();
        allChildrens = transform.GetComponentsInChildren<Transform>();        
    }
    private void Update()
    {
        if (hitPoints <= 0 && !alreadyDead )
        {
            if (_golemHand1 != null)
            {
                _golemHand1.tag = "Untagged";
                _golemHand2.tag = "Untagged";
            }
            else
            {
                foreach (Transform _object in allChildrens)
                {
                    if (_object.CompareTag(_tag))
                    {
                        Debug.Log("Тег найден, обнулен");
                        _object.tag = "Untagged";
                    }
                }
            }            
            if (animator != null) animator.SetTrigger("Death");
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    switch (param.type)
                    {
                        case AnimatorControllerParameterType.Float:
                            animator.SetFloat(param.name, 0f);
                            break;
                        case AnimatorControllerParameterType.Int:
                            animator.SetInteger(param.name, 0);
                            break;
                        case AnimatorControllerParameterType.Bool:
                            animator.SetBool(param.name, false);
                            break;
                    }
                }
            }

            if (_enemyStateManager != null) { _enemyStateManager.enabled = false; }
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = false; }
            alreadyDead = true;
        }
        if (_playerDamage.hitPoints <= 0)
        {
            transform.position = startCoordination ;
            transform.rotation = startRotation;
            hitPoints = startHitPoints;

            if (_enemyStateManager != null) { _enemyStateManager.enabled = true; }
            if (_zoneTriggerManager != null) { _zoneTriggerManager.enabled = true; }

            revive = true;
            
            if (animator != null)
            {
                foreach (AnimatorControllerParameter param in animator.parameters)
                {
                    switch (param.type)
                    {
                        case AnimatorControllerParameterType.Float:
                            animator.SetFloat(param.name, 0f);
                            break;
                        case AnimatorControllerParameterType.Int:
                            animator.SetInteger(param.name, 0);
                            break;
                        case AnimatorControllerParameterType.Bool:
                            animator.SetBool(param.name, false);
                            break;
                    }
                }
            }
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Idle", 0);

            alreadyDead = false;
        }
    }
    public IEnumerator CanHit()
    {
        if (_golemHand1 != null)
        {
            _golemHand1.tag = "Untagged";
            _golemHand2.tag = "Untagged";
            yield return new WaitForSeconds(timeNoAttaking);
            _golemHand1.tag = _tag;
            _golemHand2.tag = _tag;
        }
        else
        {
            GameObject weapon = null;
            foreach (Transform _object in allChildrens)
            {
                if (_object.CompareTag(_tag))
                {
                    if (weapon == null)
                    {
                        weapon = _object.gameObject;
                    }
                    _object.tag = "Untagged";
                }
            }
            yield return new WaitForSeconds(timeNoAttaking);
            weapon.tag = _tag;
            weapon = null;
        }
        
    }
}
