using UnityEngine;

public class PlitaTrigger : MonoBehaviour
{
    [SerializeField] private float speed = 4.5f;
    private bool shouldMove = false;
    public static int boxCount = 0;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position + new Vector3(0f, -0.2f, 0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box_Key")) { boxCount++; shouldMove = true; }
        else return;
    }

    private void Update()
    {
        if (shouldMove && transform.position != targetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box_Key")) { boxCount--; shouldMove = false; }
        else return;
    }
}

