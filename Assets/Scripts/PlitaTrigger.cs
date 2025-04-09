using UnityEngine;

public class PlitaTrigger : MonoBehaviour
{
    [SerializeField] GameObject plita;
    [SerializeField] private float speed = 4.5f;
    private bool shouldMove = false;
    public static int boxCount = 0;
    private Vector3 targetPosition;
    private bool hasMove = false;

    private void Start() {
        targetPosition = plita.transform.position;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Box_Key")) { boxCount++; targetPosition = plita.transform.position + new Vector3(0f, -0.2f, 0f); shouldMove = true; }
        else return;
    }

    private void Update() {
        if (shouldMove && !hasMove) 
        {
            plita.transform.position = Vector3.Lerp(plita.transform.position, targetPosition, speed * Time.deltaTime);
        }
        if (Vector3.Distance(plita.transform.position, targetPosition) < 0.01f) { shouldMove = false; }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Box_Key")) { boxCount--; hasMove = true; }
        else return;
    }
}
