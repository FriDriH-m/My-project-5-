using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] GameObject keyModel;
    private void Start()
    {
        keyModel.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyModel.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyModel.SetActive(false);
        }
    }
}
