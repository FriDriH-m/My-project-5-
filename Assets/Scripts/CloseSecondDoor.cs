using UnityEngine;

public class CloseSecondDoor : MonoBehaviour
{
    SecondDoorTrigger triggerDoor;
    private void Start()
    {
        triggerDoor = FindFirstObjectByType<SecondDoorTrigger>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerDamage>() != null)
        {
            Debug.Log("Игрока");
            if (triggerDoor != null)
            {
                triggerDoor.toClose = true;
            }
            else Debug.Log("No script");
        }
    }
}
