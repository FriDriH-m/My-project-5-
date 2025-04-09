using UnityEngine;

public class BoxBreak : MonoBehaviour
{
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject boxPart;
    [SerializeField] private float breakSpeed = 1f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed > breakSpeed) { Destroy(boxPart); Destroy(box); }
    }
}
