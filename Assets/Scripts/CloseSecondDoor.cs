using UnityEngine;

public class CloseSecondDoor : MonoBehaviour
{
    SecondDoorTrigger triggerDoor;
    private void Start() => triggerDoor = FindFirstObjectByType<SecondDoorTrigger>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("R_Hand") || other.CompareTag("L_Hand"))
        {
            if (triggerDoor != null)
            {
                triggerDoor.toClose = true;
            }
        }
    }
}
