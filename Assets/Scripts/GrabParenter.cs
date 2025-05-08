using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] GameObject train;
    [SerializeField] GameObject hint;
    [SerializeField] Vector3 _offset;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    [SerializeField] private Vector3 _leftHandPosition;
    [SerializeField] private Vector3 _leftHandEuler;
    [SerializeField] private Vector3 _rightHandPosition;
    [SerializeField] private Vector3 _rightHandEuler;
    private Quaternion _leftHandRotation => Quaternion.Euler(_leftHandEuler);
    private Quaternion _rightHandRotation => Quaternion.Euler(_rightHandEuler);

    Rigidbody rb;

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

        if (interactor.CompareTag("L_Hand"))
        {
            _leftHand.transform.SetParent(interactable);
            _leftHand.transform.localPosition = _leftHandPosition;
            _leftHand.transform.localRotation = _leftHandRotation;
        }
        else if (interactor.CompareTag("R_Hand"))
        {
            _rightHand.transform.SetParent(interactable);
            _rightHand.transform.localPosition = _rightHandPosition;
            _rightHand.transform.localRotation = _rightHandRotation;
        }

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
        if (interactor.CompareTag("L_Hand"))
        {
            _leftHand.transform.SetParent(interactor);
            _leftHand.transform.localPosition = Vector3.zero;
            _leftHand.transform.localRotation = Quaternion.Euler(-180, 167.888f, -90);
        }
        else if (interactor.CompareTag("R_Hand"))
        {
            _rightHand.transform.SetParent(interactor);
            _rightHand.transform.localPosition = Vector3.zero;
            _rightHand.transform.localRotation = Quaternion.Euler(-180, 187.945f, 90);
        }
    }
}

