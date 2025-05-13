using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public Vector3 reviveCoordination;
    [SerializeField] public int hitPoints = 200;
    private WeaponDamage _weaponDamage;
    private void OnTriggerEnter(Collider other)
    {
        Transform parent = transform.parent;
        _weaponDamage = parent.GetComponentInChildren<WeaponDamage>();        

        if (other.gameObject.CompareTag("Great_Sword"))
        {
            //Debug.Log("Great Sword урон");
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else hitPoints -= 60;
            }
            else hitPoints -= 60;
        }
        else if (other.gameObject.CompareTag("Sword"))
        {
            //Debug.Log("Sword урон");
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else hitPoints -= 30;
            }
            else hitPoints -= 30;
        }
        else if (other.gameObject.CompareTag("Axe"))
        {
            //Debug.Log("Axe урон");
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    Debug.Log("Урон не прошел, блокировал");
                    return;
                }
                else hitPoints -= 20;
            }
            else hitPoints -= 20;
        }
        else if (other.gameObject.CompareTag("Golem"))
        {
            //Debug.Log("Golem урон");
            if (_weaponDamage != null)
            {
                if (_weaponDamage._touchSword)
                {
                    hitPoints -= 50;
                    return;
                }
                else hitPoints -= 90;
            }
            else hitPoints -= 90;
        }

        if (hitPoints <= 0)
        {
            StartCoroutine(Revive());
            //Смерть игрока
        }

    }
    private IEnumerator Revive()
    {
        yield return new WaitForSeconds(2);
        hitPoints = 200;
        transform.parent.position = reviveCoordination;
    }
}
