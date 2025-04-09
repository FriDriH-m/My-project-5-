using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] GameObject train;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) train.SetActive(false);
    }
}
