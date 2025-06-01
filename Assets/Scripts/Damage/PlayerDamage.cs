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
    [SerializeField] private Image fadeOverlay; 
    [SerializeField] private float fadeInDuration = 2.0f;
    public GameObject DeathCanvas;
    public BossSound BossSound;
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
            //Debug.Log("Âðàã íå àòàêóåò");
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
                    //Debug.Log("Óðîí íå ïðîøåë, áëîêèðîâàë");
                    return;
                }
                else
                {
                    //Debug.Log("Great Sword óðîí");
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
            //Debug.Log("Sword óðîí");
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Óðîí íå ïðîøåë, áëîêèðîâàë");
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
            //Debug.Log("Axe óðîí");
            if (_damageCount != null)
            {
                _damageCount.StartCoroutine(_damageCount.CanHit());
            }

            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
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
            //Debug.Log("Golem óðîí");
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
            //Ñìåðòü èãðîêà
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
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // ×óòü ìåíÿåì òîí
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
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // ×óòü ìåíÿåì òîí
            audioSource.PlayOneShot(hitGolemSounds[randomSoundIndex]);
        }
    }
    public IEnumerator Revive()
    {
        StartFadeEffect();
        yield return new WaitForSeconds(0.1f);
        Deaths+=1;
        hitPoints = 200;
        BossSound.flagForSound = true;
        FindFirstObjectByType<BossSound>().StartFadeOut();
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
        DeathCanvas.SetActive(true);
        float elapsedTime = 0f;
        Color startColor = fadeOverlay.color;
        Color targetColor = new Color(0, 0, 0, 0); // Ïîëíàÿ ïðîçðà÷íîñòü
        yield return new WaitForSeconds(1f);
        while (elapsedTime < fadeInDuration)
        {
            fadeOverlay.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.color = targetColor; // Ôèíàëèçèðóåì ïðîçðà÷íîñòü
        DeathCanvas.SetActive(false);
    }
}
