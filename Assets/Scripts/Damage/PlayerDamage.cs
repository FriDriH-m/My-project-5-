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
    public AudioClip[] hitGolemSounds;
    public int Deaths = 0;
    public bool Damage = false;
    public AudioClip Hit;
    public DataAchievement Icon16;
    [SerializeField] private Image fadeOverlay; // Перетащите сюда FadeOverlay
    [SerializeField] private float fadeInDuration = 2.0f; // Длительность проявления
    private void Start()
    {
        hitPoints = 200f;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = transform.parent;
        _weaponDamage = parent.GetComponentInChildren<WeaponDamage>();
        DamageCount _damageCount = other.GetComponentInParent<DamageCount>();
        if (_damageCount != null && !_damageCount.attacking)
        {
            //Debug.Log("Враг не атакует");
            return;
        }
        if (other.gameObject.CompareTag("Great_Sword"))
        {
            
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    //Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else
                {
                    //Debug.Log("Great Sword урон");
                    PlayerHit();
                    hitPoints -= 60;
                }
            }
            else { hitPoints -= 60; PlayerHit(); } 
            Damage = true;
            HitSound();
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
                    hitPoints -= 30;
                    PlayerHit();
                }
            }
            else
            {
                hitPoints -= 30; 
                PlayerHit();
            }

            Damage = true;
            HitSound();
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
                    hitPoints -= 20;
                    PlayerHit();
                }
            }
            else { hitPoints -= 20; PlayerHit(); }
            Damage = true;
            HitSound();
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
                 hitPoints -= 90;
                 PlayerHit();
            }
            else { hitPoints -= 90; PlayerHit(); }
            Damage = true;
            HitGolemSound();
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
    private void PlayerHit()
    {
        audioSource.PlayOneShot(Hit);
    }
    public void HitGolemSound()
    {
        if (hitGolemSounds.Length > 0)
        {
            int randomSoundIndex = Random.Range(0, hitGolemSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // Чуть меняем тон
            audioSource.PlayOneShot(hitGolemSounds[randomSoundIndex]);
        }
    }
    public IEnumerator Revive()
    {
        StartFadeEffect();
        yield return new WaitForSeconds(0.1f);
        Deaths+=1;
        hitPoints = 200;
        HealthBar();
        transform.parent.position = reviveCoordination;
    }

    public void StartFadeEffect()
    {
        fadeOverlay.color = new Color(0, 0, 0, 1);
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color startColor = fadeOverlay.color;
        Color targetColor = new Color(0, 0, 0, 0); // Полная прозрачность

        while (elapsedTime < fadeInDuration)
        {
            fadeOverlay.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = targetColor; // Финализируем прозрачность
    }
}
