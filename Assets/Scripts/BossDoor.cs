using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;

    public GameObject keyModel;
    public bool inZone = false;
    public Material myMaterial;
    private void Start()
    {
        keyModel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            inZone = true;
            keyModel.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {        
        if (other.CompareTag("Key"))
        {
            inZone = false;
            keyModel.SetActive(false);
        }
    }

    private void Update()
    {
        if (shouldMove)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(door.transform.position, targetPosition) < 0.01f) shouldMove = false;
    }
    public IEnumerator KeyActive()
    {
        inZone = false;
        Renderer renderer = keyModel.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = myMaterial;
        }
        shouldMove = true;
        targetPosition = door.position + new Vector3(0, -15, 0);
        FindFirstObjectByType<PreBossSound>().StartFadeOut();
        yield return null;
    }
}
