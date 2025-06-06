using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SecondDoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float moveSpeed;
    [SerializeField] List<GameObject> objectsToDestroy;
    private bool shouldMove = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    public AudioSource audioSource;
    private bool wasMoved = false;
    public bool toClose = false;
    public bool FlagForSound;
    public void Start()
    {
        startPosition = transform.position;
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
        if (toClose)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, startPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(door.transform.position, startPosition) < 0.1f) 
            { 
                toClose = false; 
                foreach (GameObject obj in objectsToDestroy)
                {
                    Destroy(obj);
                }
            }
        }
        if (shouldMove)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (shouldMove && Vector3.Distance(door.transform.position, targetPosition) < 0.3f) shouldMove = false;


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