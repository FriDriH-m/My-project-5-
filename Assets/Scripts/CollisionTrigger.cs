using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("R_Hand") && !other.CompareTag("L_Hand")) return;
        else GameObject.SetActive(false);
    }
}
