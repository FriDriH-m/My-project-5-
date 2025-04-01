using UnityEngine;

public class Collisiontrigger : MonoBehaviour
{
    [SerializeField] private GameObject GameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("R_Hand") && !other.CompareTag("L_Hand"))
        {
            return;
        }
        GameObject.SetActive(false);
    }
}
