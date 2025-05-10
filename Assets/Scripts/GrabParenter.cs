using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] private Transform _leftHand; // модельк левой руки
    [SerializeField] private Transform _rightHand; // моделька правой руки
    [SerializeField] private bool _canHoldTwoHands; // можно ли держать оружие двумя руками

    [SerializeField] private Vector3 _leftHandPosition;  //
    [SerializeField] private Vector3 _leftHandEuler;     //  позиционирование моделек рук на рукоятке оружия после граба
    [SerializeField] private Vector3 _rightHandPosition; //
    [SerializeField] private Vector3 _rightHandEuler;    //
    [SerializeField] private Vector3 _twoHandEuler; // вращение в двуручном хвате
    private Quaternion _leftHandRotation => Quaternion.Euler(_leftHandEuler); // преобразование Vector3 в Quaternion и запись в переменную
    private Quaternion _rightHandRotation => Quaternion.Euler(_rightHandEuler); // преобразование Vector3 в Quaternion и запись в переменную
    private Quaternion _twoHandRotation => Quaternion.Euler(_twoHandEuler); // преобразование Vector3 в Quaternion и запись в переменную
    private XRGrabInteractable grabInteractable;
    private Transform _firstHand; // переменная которая будет хранить какая рука схватила первой оружиея
    private Transform _secondaryHand; // переменная которая будет хранить какая рука схватила второй оружие
    Rigidbody rb;
    Transform interactable;
    Transform interactor;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _offset;
    }
    public void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        if (_firstHand == null)
        {
            _firstHand = interactor;
            interactable.SetParent(interactor); // задается родитель в виде Near-Far Interactore в соответсвующем контроллере
            HandToTargetPosition(_firstHand);
        }
        if (_canHoldTwoHands)
        {
            if (interactor.transform != _firstHand)
            {
                _secondaryHand = interactor;
                if (_firstHand.CompareTag("R_Hand"))
                {
                    _leftHand.GetComponent<Collider>().enabled = false;
                    _leftHand.transform.SetParent(interactable);
                    _leftHand.transform.localPosition = _leftHandPosition - new Vector3(0, 0.2f, 0);
                    _leftHand.transform.localRotation = _leftHandRotation;
                }
                else
                {
                    _rightHand.GetComponent<Collider>().enabled = false;
                    _rightHand.transform.SetParent(interactable);
                    _rightHand.transform.localPosition = _rightHandPosition - new Vector3(0, 0.2f, 0);
                    _rightHand.transform.localRotation = _rightHandRotation;
                }
                TwoHandGrab();
            }
        }   
        //rb.linearDamping = 70f;
        //rb.angularDamping = 70f;
    }

    public void OnUngrab(SelectExitEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        if (_firstHand != null && _secondaryHand != null)
        {
            if (args.interactorObject.transform == _firstHand)
            {
                HandToStartPosition(_firstHand);
                _firstHand = _secondaryHand;
                interactable.SetParent(interactor);
                _secondaryHand = null;
                grabInteractable.trackPosition = true;
                grabInteractable.trackRotation = true;
            }
            else if (args.interactorObject.transform == _secondaryHand)
            {
                HandToStartPosition(_secondaryHand);
                _secondaryHand = null;
            }
        }
        else 
        {
            Debug.Log(_firstHand);
            HandToStartPosition(_firstHand);            
            _firstHand = null;
            interactable.SetParent(null);
        }
    }
    public void TwoHandGrab()
    {
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }
    public void HandToStartPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            _leftHand.GetComponent<Collider>().enabled = true;
            _leftHand.transform.SetParent(interactor);
            _leftHand.transform.localPosition = Vector3.zero;
            _leftHand.transform.localRotation = Quaternion.Euler(-180, 167.888f, -90);
        }
        else if (hand.CompareTag("R_Hand"))
        {
            _rightHand.GetComponent<Collider>().enabled = true;
            _rightHand.transform.SetParent(interactor);
            _rightHand.transform.localPosition = Vector3.zero;
            _rightHand.transform.localRotation = Quaternion.Euler(-180, 187.945f, 90);
        }
    }
    public void HandToTargetPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            _leftHand.GetComponent<Collider>().enabled = false;
            _leftHand.transform.SetParent(interactable);
            _leftHand.transform.localPosition = _leftHandPosition;
            _leftHand.transform.localRotation = _leftHandRotation;
        }
        else if (hand.CompareTag("R_Hand"))
        {
            _rightHand.GetComponent<Collider>().enabled = false;
            _rightHand.transform.SetParent(interactable);
            _rightHand.transform.localPosition = _rightHandPosition;
            _rightHand.transform.localRotation = _rightHandRotation;
        }
    }
    private void Update()
    {
        if (_firstHand != null && _secondaryHand != null)
        {
            Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, 0.5f);
            transform.position = midPoint;

            Vector3 direction = _firstHand.position - _secondaryHand.position;
            transform.rotation = Quaternion.LookRotation(direction) * _twoHandRotation;
        }
    }
}

