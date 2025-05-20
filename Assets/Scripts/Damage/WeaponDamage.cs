using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody; 
    [SerializeField] private float _damageRatio = 10f;
    [SerializeField] private float _minimalImpuls;
    [SerializeField] private ParticleSystem _effect;
    public bool _touchSword = false; 
    private bool _canHit = true;
    private Coroutine _canHitCoroutine = null;
    float _instImpuls;

    float _impulsValue 
    { 
        get { return _instImpuls; } 
        set { _instImpuls = Mathf.Min(value, 80f);} 
    }


    private void Awake()
    {  
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_canHit) return;

        float impuls = collision.impulse.magnitude;
        //Debug.Log(impuls);

        HitZone hitZone = collision.collider.GetComponentInParent<HitZone>();
        DamageCount _damageCount = collision.collider.GetComponentInParent<DamageCount>();
        Animator _animator = collision.collider.GetComponentInParent<Animator>();

        _impulsValue = impuls * _damageRatio;

        if (hitZone != null && _canHit) 
        {
            //Debug.Log("Импульс - " + _instImpuls);
            if (hitZone.zone == HitZone.ZoneType.Sword)
            {
                if (_touchSword == false) StartCoroutine(SwordTouch());
                if (_instImpuls > _minimalImpuls)
                {
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Instantiate(_effect, contact.point, Quaternion.identity);
                        return;
                    }
                }

            }
            if (hitZone.haveArmor) _instImpuls *= 0.8f;
            if (hitZone.zone == HitZone.ZoneType.Head)
            {
                _instImpuls *= 1.5f;
                if (_touchSword) _instImpuls *= 0.2f;

                if (_instImpuls > _minimalImpuls)
                {
                    //Debug.Log("ГОЛОВА \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Instantiate(_effect, contact.point, Quaternion.identity);
                        return;
                    }
                    _damageCount.hitPoints -= _instImpuls;
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    if (_animator != null) _animator.SetBool("HeadImpact", true);
                    return;
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Torso)
            {
                _instImpuls *= 1.2f;
                if (_touchSword) _instImpuls *= 0.2f;
                if (_instImpuls > _minimalImpuls)
                {
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    //Debug.Log("ТУЛОВИЩЕ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Instantiate(_effect, contact.point, Quaternion.identity);
                        return;
                    }
                    _damageCount.hitPoints -= _instImpuls;
                    if (_animator != null) _animator.SetBool("TorsoImpact", true);
                    return;
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Limbs)
            {
                _instImpuls *= 0.8f;
                if (_touchSword) _instImpuls *= 0.2f; 

                if (_instImpuls > _minimalImpuls)
                {
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    //Debug.Log("КОНЕЧНОСТЬ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Instantiate(_effect, contact.point, Quaternion.identity);
                        return;
                    }
                    _damageCount.hitPoints -= _instImpuls;
                    return;
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Shield)
            {
                _instImpuls *= 0.1f;
                
                if (_instImpuls > _minimalImpuls)
                {
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    //Debug.Log("ЩИТ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Instantiate(_effect, contact.point, Quaternion.identity);
                        return;
                    }
                    return;
                }
            }
        }
    }
    private IEnumerator ExitHitZone()
    {
        _canHit = false;
        yield return new WaitForSeconds(0.7f);
        _canHit = true;
        _canHitCoroutine = null;
    }
    private IEnumerator SwordTouch()
    {
        _touchSword = true;
        yield return new WaitForSeconds(0.8f);
        _touchSword = false;
    }
    private void TouchEffect()
    {

    }
}
