using UnityEngine;

public class Abyss : MonoBehaviour
{
    public PlayerDamage _playerDamage;
    DataAchievement Icon8;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DamageController")
        {
            _playerDamage.hitPoints = 0f;
            _playerDamage.HealthBar();
            _playerDamage.Revive();
            if (Icon8._unlocked == false)
            Icon8._unlocked = true;
        }
    }
}
