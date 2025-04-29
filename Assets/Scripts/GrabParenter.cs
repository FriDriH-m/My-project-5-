using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Unity.VisualScripting;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] GameObject train;
    [SerializeField] GameObject hint;
    [SerializeField] Vector3 _offset;
    [SerializeField] LayerMask _layerMask;
    ConfigurableJoint configurableJoint;
    Rigidbody rb;
    GameObject joint;

    Transform interactable;
    Transform interactor;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _offset;
    }
    private void OnDrawGizmos()
    {
        rb = GetComponent<Rigidbody>();        
    }
    public void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;

        interactable.SetParent(interactor);
        rb = interactable.GetComponent<Rigidbody>();
        

        if (gameObject.CompareTag("Start_Weapon"))
        {
            train.SetActive(true);
        }
        if (gameObject.CompareTag("Secret_Weapon"))
        {
            hint.SetActive(true);
        }
    }

    public void OnUngrab(SelectExitEventArgs args)
    {
        transform.SetParent(null);
    }
}

