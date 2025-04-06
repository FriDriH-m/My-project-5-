using UnityEngine;

public class PlitaTrigger : MonoBehaviour
{
    public static int boxCount = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box_Key")) { boxCount++; Debug.Log(boxCount); }
        else return;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box_Key")) { boxCount--; Debug.Log(boxCount); }
        else return;
    }
}
