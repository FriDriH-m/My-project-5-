using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public static int targetCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow")) targetCount++;
    }
}
