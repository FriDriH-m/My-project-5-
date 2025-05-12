using UnityEngine;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class FernPhysicsHandler : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _wasGrabbed = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnGrabbed()
    {
        if (!_wasGrabbed)
        {
            _rigidbody.isKinematic = false;
            _wasGrabbed = true;
        }
    }
}
