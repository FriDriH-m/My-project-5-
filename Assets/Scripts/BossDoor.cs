using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class BossDoor : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] private float moveSpeed;
    private bool shouldMove = false;
    private Vector3 targetPosition;
    public bool FlagForSound;
    public GameObject keyModel;
    public bool inZone = false;
    public Material myMaterial;
    public AudioSource audioSource;
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
