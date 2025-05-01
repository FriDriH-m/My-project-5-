using UnityEngine;

public class AttackLeftZone : MonoBehaviour
{
    public bool haveWeapon = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            //Debug.Log("Weapon detected in AttackLeftZone");
            haveWeapon = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            haveWeapon = false;
        }
    }
}
