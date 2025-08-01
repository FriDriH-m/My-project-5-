using UnityEngine;
using UnityEngine.UI;

public class Healing : MonoBehaviour
{
    [SerializeField] public Collider _podorozhniktrigger; //�������, ������� ������ ���� ����������� (�� MainCamera)
    [SerializeField] private int _healAmount; // ��������� ��������
    public DataAchievement Icon7;
    [SerializeField] private AudioClip Eat;
    [SerializeField] private AudioSource audioSource;

    private PlayerDamage _playerDamage;
    public Image Bar;
    private void Start()
    {
        _playerDamage = FindFirstObjectByType<PlayerDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Item item = GetComponent<Item>();
        if (other != _podorozhniktrigger) return;
        if (item.inSlot == true) return;
        if (_playerDamage.hitPoints == 200) return;
        ApplyHealing();
        DisableBush();
    }

    private void ApplyHealing() // ��������� ��, �� �� ������ 200. ���� � ��� ������������ ��, �� ���������
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

    public void OnGrabAchiv()
    {
        Icon7._unlocked = true;
    }

    private void DisableBush() // �������� ����������, ���� ������ �� ��������
    {
        audioSource.PlayOneShot(Eat);
        Destroy(gameObject);
    }
}