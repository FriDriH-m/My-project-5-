using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    int hitPoints = 200;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Great_Sword"))
        {
            hitPoints -= 60;
        }
        if (other.gameObject.CompareTag("Sword"))
        {
            hitPoints -= 30;
        }
        if (other.gameObject.CompareTag("Axe"))
        {
            hitPoints -= 20;
        }
        if (other.gameObject.CompareTag("Golem"))
        {
            hitPoints -= 90;
        }
    }
}
