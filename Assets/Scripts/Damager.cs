using UnityEngine;

public class Damager : MonoBehaviour
{
    private Rigidbody rb;
    public float damage;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            float impactSpeed = collision.relativeVelocity.magnitude;
        }
    }
}
