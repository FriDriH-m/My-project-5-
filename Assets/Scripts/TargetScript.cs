using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public static int targetCount = 0;
    private bool wasShooted = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Arrow") && !wasShooted)
        {
            Debug.Log("Попал!");
            targetCount++;
            wasShooted = true;
        }
    }
}
