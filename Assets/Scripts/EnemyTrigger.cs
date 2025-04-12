using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {

        }
    }
}
