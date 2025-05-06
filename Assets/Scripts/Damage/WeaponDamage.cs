using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private float _mass = 1; // масса меча
    [SerializeField] private Rigidbody _rigidBody; // rigidbody меча, чтобы вычислить его скорость
    private Animator _animator;
    private XRGrabInteractable _interactable;
    private DamageCount _damageCount; // скрипт на враге с его здоровьем
    public float speed = 0; // скорость меча
    public float impuls = 0; // импыльс = скорость меча * массу меча
    bool _touchSword = false; // было ли касание об меч врага
    float _timeAfterSwrdTch = 0; // врем€, которое прошло после последнего касани€ с мечом


    private void Awake()
    {  
        _interactable = GetComponent<XRGrabInteractable>();
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        speed = _rigidBody.linearVelocity.magnitude;
        impuls = _mass * speed;
        if (_touchSword)
        {
            _timeAfterSwrdTch += Time.deltaTime;
        }
        if (_timeAfterSwrdTch > 0.4f)
        {
            _timeAfterSwrdTch = 0f;
            _touchSword = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //_interactable.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        HitZone hitZone = collision.collider.GetComponentInParent<HitZone>();
        _damageCount = collision.gameObject.GetComponent<DamageCount>();
        _animator = collision.gameObject.GetComponent<Animator>();
        float _instImpuls = impuls;
        

        if (hitZone != null)
        {
            if (_instImpuls > 20)
            {
                Debug.Log("»мпульс - " + _instImpuls);
            }
            if (hitZone.zone == HitZone.ZoneType.Sword)
            {
                _touchSword = true;
            }
            if (hitZone.haveArmor)
            {
                _instImpuls *= 0.8f;
            }
            if (hitZone.zone == HitZone.ZoneType.Head)
            {
                _instImpuls *= 1.5f;
                if (_touchSword)
                {
                    _instImpuls *= 0.2f;
                }

                if (_instImpuls > 20f)
                {

                    Debug.Log("√ќЋќ¬ј \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                    if (_animator != null)
                    {
                        _animator.SetBool("HeadImpact", true);
                    }                    
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Torso)
            {
                _instImpuls *= 1.2f;
                if (_touchSword)
                {
                    _instImpuls *= 0.2f;
                }

                if (_instImpuls > 20f)
                {
                    Debug.Log("“”Ћќ¬»ў≈ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                    if (_animator != null)
                    {
                        _animator.SetBool("TorsoImpact", true);                       
                    }                    
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Limbs)
            {
                _instImpuls *= 0.8f;
                if (_touchSword)
                {
                    _instImpuls *= 0.2f;
                }

                if (_instImpuls > 20f)
                {
                    Debug.Log(" ќЌ≈„Ќќ—“№ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Shield)
            {
                _instImpuls *= 0f;
                if (_instImpuls > 20f)
                {
                    Debug.Log("ў»“ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //_interactable.movementType = XRBaseInteractable.MovementType.Kinematic;
    }
}
