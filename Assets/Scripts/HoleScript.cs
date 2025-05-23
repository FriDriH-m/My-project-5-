using UnityEngine;

public class HoleScript : MonoBehaviour
{
    [SerializeField] private Vector3 respawnPoint;
    [SerializeField] private Transform player;
    [SerializeField] private ParticleSystem _effect;
    private Transform _parent;
    private void OnTriggerEnter(Collider other)
    {        
        Debug.Log("Something");
        if (other.transform.CompareTag("Weapon"))
        {
            FindParent(other);
            if (_parent != null) 
            {
                _parent.transform.position = player.position + new Vector3(2, 2, 0);
                _parent.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                _parent.rotation = Quaternion.identity;
                Instantiate(_effect, _parent.position, Quaternion.identity);
            } 
            else
            {
                other.transform.position = player.position + new Vector3(2, 2, 0);
                other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                other.transform.rotation = Quaternion.identity;
                Instantiate(_effect, other.transform.position, Quaternion.identity);
            }
            
            
        }
        if (other.GetComponentInChildren<PlayerDamage>() != null)
        {
            Debug.Log("player");
            other.GetComponentInChildren<PlayerDamage>().hitPoints = 0;
        }
    }

    private void FindParent(Collider other)
    {
        for (int i = 0; i < other.transform.childCount; i++)
        {
            _parent = other.transform.parent;
            if (_parent == null)
            {
                return;
            }
            else
            {
                _parent = _parent.parent;
            }
        }
    }
}
