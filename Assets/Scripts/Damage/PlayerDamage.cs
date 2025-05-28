using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    public Vector3 reviveCoordination;
    public float hitPoints;
    private WeaponDamage _weaponDamage;
    public Image Bar;
    private AudioSource audioSource;
    public AudioClip[] hitSounds;
    public int Deaths = 0;
    public bool Damage = false;
    public DataAchievement Icon16;
    private void Start()
    {
        hitPoints = 200f;
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = transform.parent;
        _weaponDamage = parent.GetComponentInChildren<WeaponDamage>();
        DamageCount _damageCount = other.GetComponentInParent<DamageCount>();
        if (_damageCount != null && !_damageCount.attacking)
        {
            Debug.Log("Враг не атакует");
            return;
        }
        if (other.gameObject.CompareTag("Great_Sword"))
        {
            //Debug.Log("Great Sword урон");            
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else
                {
                    HitSound();
                    hitPoints -= 60;
                }
            }
            else hitPoints -= 60; 
            Damage = true;
        }
        else if (other.gameObject.CompareTag("Sword"))
        {
            //Debug.Log("Sword урон");
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else
                {
                    HitSound();
                    hitPoints -= 30;
                }
            }
            else
            {
                HitSound();
                hitPoints -= 30; 
            }

            Damage = true;
        }
        else if (other.gameObject.CompareTag("Axe"))
        {
            //Debug.Log("Axe урон");
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }

            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else 
                {
                    HitSound();
                    hitPoints -= 20; 
                }
            }
            else hitPoints -= 20;
            Damage = true;

        }
        else if (other.gameObject.CompareTag("Golem"))
        {
            //Debug.Log("Golem урон");
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    hitPoints -= 50;
                    return;
                }
                else 
                { 
                    HitSound();
                    hitPoints -= 90; 
                }
            }
            else hitPoints -= 90;
            Damage = true;
        }
        HealthBar();
        if (hitPoints <= 0)
        {
            if (other.gameObject.CompareTag("Axe") && (other.gameObject.transform.root.name == "Mediavel Knight Axe Variant (1)"))
            {
                Icon16._unlocked = true;
            }
            StartCoroutine(Revive());
            //Смерть игрока
        }

    }
    public void HealthBar()
    {
        Bar.fillAmount = hitPoints / 200;
        //Debug.Log($"HealBar{Bar.fillAmount}+{hitPoints / 200}");
    }
    public void HitSound()
    {
        if (hitSounds.Length > 0)
        {
            int randomSoundIndex = Random.Range(0, hitSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // Чуть меняем тон
            audioSource.PlayOneShot(hitSounds[randomSoundIndex]);
        }
    }
    public IEnumerator Revive()
    {
        yield return new WaitForSeconds(0.1f);
        Deaths+=1;
        hitPoints = 200;
        HealthBar();
        transform.parent.position = reviveCoordination;
    }
}
