using UnityEngine;

public class AttackMiddleZone : MonoBehaviour
{
    public bool haveWeapon = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon")) haveWeapon = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon")) haveWeapon = false;
    }
}
