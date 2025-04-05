using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] GameObject plane;

    void Start()
    {
        plane.SetActive(false);
    }
}
