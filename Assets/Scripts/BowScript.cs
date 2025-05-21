using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class BowScript : MonoBehaviour
{
    Animator animator; // аниматор лука
    public bool isGrabbed; // взял ли игрок лук в руки. в GrabParenter задается true
    [SerializeField] GameObject arrowModel;
    [SerializeField] Transform arrowModelSpawn;
    [SerializeField] GameObject tetiva; // тетива лука
    [SerializeField] GameObject arrow; // префаб стрелы чтобы создавать 
    GameObject spawnedArrow; // созданная стрела
    Rigidbody rb; // ригидбади стрелы
    private bool _wasPressed = false;
    private void Start()
    {
        animator = transform.GetComponent<Animator>();
    }
    private void Update()
    {

    }
    public void Active(ActivateEventArgs args)
    {
        animator.SetBool("Shoot", true);        
    }
    private void Shoot() 
    {
        arrowModel.SetActive(false);

        Vector3 spawnPosition = transform.position + transform.right * 0.4f + transform.up * 0.3f;
        spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);
        spawnedArrow.transform.rotation = Quaternion.LookRotation(transform.up);

        StartCoroutine(DeleteArrow(spawnedArrow));

        rb = spawnedArrow.GetComponent<Rigidbody>();
        spawnedArrow.transform.SetParent(null);
        rb.AddForce(transform.right * 30, ForceMode.Impulse);
    }
    private void StartShoot()
    {
        arrowModel.SetActive(true);
    }
    private void EndShoot()
    {
        animator.SetBool("Shoot", false);
        
    }
    public IEnumerator DeleteArrow(GameObject arrow)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(arrow);
    }
}