using UnityEngine;

public class SecondDoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;
    public AudioClip doorOpenSound;
    public AudioSource audioSource;
    private bool wasMoved = false;  

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetPosition = door.transform.position;
    }
    public void OnTriggerStay(Collider other)
    {
        //Debug.Log(TargetScript.targetCount);
        if (TargetScript.targetCount == 4 && !wasMoved) { shouldMove = true; targetPosition = door.transform.position + new Vector3(0f, -6f, 0f); wasMoved = true; }
        else return;
    }
    public void Update()
    {
        if (shouldMove)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(door.transform.position, targetPosition) < 0.3f) shouldMove = false;
        if (TargetScript.targetCount == 4)
        if (shouldMove)
            audioSource.Play();
        else
            audioSource.Stop();

    }
}