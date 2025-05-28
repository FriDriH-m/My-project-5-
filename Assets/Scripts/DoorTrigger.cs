using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    [SerializeField] private GameObject door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;

    public void Start() {
        targetPosition = door.transform.position + new Vector3(0f, -0.42f, 0f); 
    }
    public void OnTriggerStay(Collider other) {
        if (PlitaTrigger.boxCount == 5) { shouldMove = true; }
        else return;
    }
    public void Update() {
        if (shouldMove) {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(door.transform.position, targetPosition) < 0.01f) shouldMove = false;
    }
}
