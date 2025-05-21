using UnityEngine;
using UnityEngine.UI;

public class Healing : MonoBehaviour
{
    [SerializeField] public Collider _podorozhniktrigger; //Триггер, который чекает вход подорожника (На MainCamera)
    [SerializeField] private int _healAmount = 30; // Насколько хиллимся

    private PlayerDamage _playerDamage;
    public Image Bar;
    private void Start()
    {
        _playerDamage = FindObjectOfType<PlayerDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != _podorozhniktrigger) return;
        if (_playerDamage.hitPoints == 200) return;
        ApplyHealing();
        DisableBush();
    }

    private void ApplyHealing() // Добавляет хп, но не больше 200. Если у нас максимальное хп, то скипается
    {
        if (_playerDamage != null)
        {
            if (_playerDamage.hitPoints + _healAmount <= 200)
                _playerDamage.hitPoints += _healAmount;
            else
                _playerDamage.hitPoints = 200;
        }
        HealthBar();
    }

    private void HealthBar()
    {
        Bar.fillAmount = _playerDamage.hitPoints / 200;
    }

    private void DisableBush() // Минусует подорожник, чтоб больше не возникал
    {
        Destroy(gameObject);
    }
}