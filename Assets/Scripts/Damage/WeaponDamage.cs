using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WeaponDamage : MonoBehaviour
{    
    [SerializeField] private float _damageRatio = 10f;
    [SerializeField] private float _minimalImpuls;
    [SerializeField] ParticleSystem _effect;
    private Rigidbody _rigidBody;
    public bool _touchSword = false; 
    private bool _canHit = true;
    private Coroutine _canHitCoroutine = null;
    float _instImpuls;
    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public DataAchievement Icon9;
    public DataAchievement Icon15;
    float _impulsValue 
    { 
        get { return _instImpuls; } 
        set { _instImpuls = Mathf.Min(value, 80f);} 
    }


    private void Awake()
    {  
        if (_rigidBody == null) _rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_canHit) return;
        string weaponName = gameObject.name;
        float impuls = collision.impulse.magnitude;

        HitZone hitZone = collision.collider.GetComponentInParent<HitZone>();
        DamageCount _damageCount = collision.collider.GetComponentInParent<DamageCount>();
        Animator _animator = collision.collider.GetComponentInParent<Animator>();

        _impulsValue = impuls * _damageRatio;

        if (hitZone != null && _canHit) 
        {
            Debug.Log("Импульс - " + _instImpuls);
            if (hitZone.zone == HitZone.ZoneType.Sword)
            {
                if (_touchSword == false) StartCoroutine(SwordTouch());
                if (_instImpuls > _minimalImpuls)
                {
                    Effects(collision);
                }

            }
            if (hitZone.haveArmor) _instImpuls *= 0.8f;
            if (hitZone.zone == HitZone.ZoneType.Head)
            {
                _instImpuls *= 1.5f;
                if (_touchSword) _instImpuls *= 0.2f;

                if (_instImpuls > _minimalImpuls)
                {
                    Debug.Log("ГОЛОВА \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    Effects(collision);
                    _damageCount.hitPoints -= _instImpuls;
                    Achievement(weaponName, _damageCount);
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    if (_animator != null) _animator.SetBool("HeadImpact", true);
                    if (_animator != null) _animator.SetTrigger("Impact");
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
                    Debug.Log("ТУЛОВИЩЕ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    Effects(collision);
                    _damageCount.hitPoints -= _instImpuls;
                    Achievement(weaponName, _damageCount);
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
                    Debug.Log("КОНЕЧНОСТЬ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    Effects(collision);
                    _damageCount.hitPoints -= _instImpuls;
                    Achievement(weaponName, _damageCount);
                    return;
                }
            }
            if (hitZone.zone == HitZone.ZoneType.Shield)
            {
                _instImpuls *= 0.1f;
                if (_instImpuls > _minimalImpuls)
                {
                    Effects(collision);
                    if (_canHitCoroutine == null) { _canHitCoroutine = StartCoroutine(ExitHitZone()); }
                    Debug.Log("ЩИТ \nбыло - " + _damageCount.hitPoints + " стало - " + (_damageCount.hitPoints - _instImpuls));
                    _damageCount.hitPoints -= _instImpuls;
                    Achievement(weaponName, _damageCount);
                    return;
                }
            }
        }
    }
    private void Effects(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Instantiate(_effect, contact.point, Quaternion.identity);

            // Рандомный звук удара
            if (hitSounds.Length > 0)
            {
                int randomSoundIndex = Random.Range(0, hitSounds.Length);
                audioSource.pitch = Random.Range(0.9f, 1.1f);  // Чуть меняем тон
                audioSource.PlayOneShot(hitSounds[randomSoundIndex]);
            }
            return;
        }
    }
    private IEnumerator ExitHitZone()
    {
        _canHit = false;
        yield return new WaitForSeconds(0.7f);
        _canHit = true;
        StopCoroutine(_canHitCoroutine);
        _canHitCoroutine = null;
    }
    private IEnumerator SwordTouch()
    {
        _touchSword = true;
        yield return new WaitForSeconds(0.8f);
        _touchSword = false;
    }
    public void Achievement(string weaponName, DamageCount _damageCount)
    {
        if (weaponName == "DragonSlayer (1)")
        {
            Icon9._unlocked = true;
        }
        if ((gameObject.transform.parent == null) && (_damageCount.hitPoints <= 0))
        {
            Icon15._unlocked = true;
        }
    }
}
