using UnityEngine;

public class SecondDoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;

    public void Start()
    {
        targetPosition = door.transform.position;
    }
    public void OnTriggerStay(Collider other)
    {
        //Debug.Log(TargetScript.targetCount);
        if (TargetScript.targetCount == 4) { shouldMove = true; targetPosition = door.transform.position + new Vector3(0f, -0.42f, 0f); }
        else return;
    }
    public void Update()
    {
        if (shouldMove)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(door.transform.position, targetPosition) < 0.01f) shouldMove = false;
    }
}