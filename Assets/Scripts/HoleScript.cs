using UnityEngine;

public class HoleScript : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private Transform player;
    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log("Something");
        if (other.transform.CompareTag("Weapon"))
        {
            Debug.Log("Weapon");
            other.transform.position = player.position + new Vector3(0.3f, 0.3f, 0.3f);
        }
        if (other.GetComponentInChildren<PlayerDamage>() != null)
        {
            Debug.Log("player");
            other.GetComponent<PlayerDamage>().hitPoints = 0;
        }
    }
}
