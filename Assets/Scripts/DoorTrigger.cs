using UnityEngine;
using UnityEngine.Audio;

public class DoorTrigger : MonoBehaviour {
    [SerializeField] private GameObject door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;
    private bool wasMoved = false;
    public AudioSource audioSource;
    bool FlagForSound = false;
    public void Start() {
        targetPosition = door.transform.position + new Vector3(0f, -8f, 0f); 
    }
    public void OnTriggerStay(Collider other) {
        if (PlitaTrigger.boxCount >= 5 && !wasMoved) { shouldMove = true; wasMoved = true; }
        else return;
    }
    public void Update() 
    {
        if (shouldMove) {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(door.transform.position, targetPosition) < 0.5f)
        {
            shouldMove = false;
        }

        if (shouldMove)
        {
            if (!FlagForSound)
            {
                audioSource.Play();
                FlagForSound = true;
            }
        }
        else
            audioSource.Stop();
    }
}
