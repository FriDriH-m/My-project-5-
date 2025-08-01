using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] protected Vector3 _offset; // �� �����
    [SerializeField] protected LayerMask _layerMask; // �� �����
    [SerializeField] protected Transform _leftHand; // ������� ����� ����
    [SerializeField] protected Transform _rightHand; // �������� ������ ����
    [SerializeField] protected bool _canHoldTwoHands; // ����� �� ������� ������ ����� ������

    [SerializeField] protected Vector3 _leftHandPosition;  //
    [SerializeField] protected Vector3 _leftHandEuler;     //  ���������������� ������� ��� �� �������� ������ ����� �����
    [SerializeField] protected Vector3 _rightHandPosition; //
    [SerializeField] protected Vector3 _rightHandEuler;    //
    [SerializeField] protected Vector3 _secondHandDown; // �������� � ��������� �����
    [SerializeField] protected float _trackPoint = 0.5f;
    protected Quaternion _leftHandRotation => Quaternion.Euler(_leftHandEuler); // �������������� Vector3 � Quaternion � ������ � ����������
    protected Quaternion _rightHandRotation => Quaternion.Euler(_rightHandEuler); // �������������� Vector3 � Quaternion � ������ � ����������
    protected SecondHandGrabDistance grabInteractable;
    protected Transform _firstHand; // ���������� ������� ����� ������� ����� ���� �������� ������ �������
    protected Transform _secondaryHand; // ���������� ������� ����� ������� ����� ���� �������� ������ ������
    protected Rigidbody rb;
    protected Transform interactable;
    protected Transform interactor;
    protected Item item;
    protected Slot currentSlot;
    [SerializeField] protected AudioClip _selectItem;
    [SerializeField] protected AudioClip Grab;
    [SerializeField] protected AudioSource _audioSource;

    protected virtual void Awake()
    {
        grabInteractable = GetComponent<SecondHandGrabDistance>();
        item = GetComponent<Item>();
    }

    protected virtual void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _offset;
    }
    protected virtual void Start()
    {
        if (!_canHoldTwoHands)
        {
            grabInteractable.selectMode = InteractableSelectMode.Single;
            grabInteractable.focusMode = InteractableFocusMode.Single;
        }
        else
        {
            grabInteractable.selectMode = InteractableSelectMode.Multiple;
            grabInteractable.focusMode = InteractableFocusMode.Multiple;
        }
    }

    public virtual void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        _audioSource.PlayOneShot(Grab);
        if (item != null && item.inSlot)
        {
            interactable.localScale = item.originalScale; // ���������� �������� ������
            interactable.localEulerAngles = item.originalRotation;
            interactable.localPosition -= item.PositionOffset;
            _audioSource.PlayOneShot(_selectItem);
            currentSlot = item.currentSlot;
            if (currentSlot != null)
            {
                currentSlot.ItemInSlot = null;
            }
            // ������ ����� ����� �������
            if (_firstHand == null)
            {
                _firstHand = interactor;
                SendHandModel(_firstHand);
                interactable.SetParent(interactor); // �������� �������� � ���� Near-Far Interactore � �������������� �����������
                HandToTargetPosition(_firstHand);   // ����� ������ ������� ��� ���
            }
            // ����� ����� ����� �������

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // �������� ������
            }

            item.inSlot = false;

            if (currentSlot != null)
            {
                currentSlot.ResetColor();
            }
            item.currentSlot = null;
            StartCoroutine(EnableCollision(0.1f));
        }

        if (_firstHand == null)
        {
            _firstHand = interactor;
            SendHandModel(_firstHand);
            interactable.SetParent(interactor); // �������� �������� � ���� Near-Far Interactore � �������������� �����������
            HandToTargetPosition(_firstHand);
            SetRigidbodyDumping(70f);
        }
        if (_canHoldTwoHands)
        {
            //Debug.Log(Vector3.Distance(_firstHand.position, interactor.position));
            if (interactor.transform != _firstHand)
            {
                _secondaryHand = interactor;
                if (_firstHand.CompareTag("R_Hand"))
                {
                    if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; }
                    _leftHand.transform.SetParent(interactable);
                    _leftHand.transform.localPosition = _leftHandPosition - _secondHandDown;
                    _leftHand.transform.localRotation = _leftHandRotation;
                }
                else
                {
                    if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
                    _rightHand.transform.SetParent(interactable);
                    _rightHand.transform.localPosition = _rightHandPosition - _secondHandDown;
                    _rightHand.transform.localRotation = _rightHandRotation;
                }
                TwoHandGrab(false);
                SetRigidbodyDumping(40f);
            }
        }   
    }

    public virtual void OnUngrab(SelectExitEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // �������� ������
        }
        if (_firstHand != null && _secondaryHand != null) // ���� ��� ���������� ������ ����� ������ ������
        {
            if (interactor == _firstHand) // ���� ��������� ������ ����, ������� ������� �� ������
            {
                HandToStartPosition(_firstHand); // �������� ��������� �������, �� ���� �� ������ ����������� :,(
                _firstHand = _secondaryHand; // _firstHand ��������� �������� _secondaryHand ����� � ���������� ���� ���������� ����� ��������� ����� �� ���� ������
                SendHandModel(_firstHand);
                interactable.SetParent(_secondaryHand); // �������� ���� ���������� �������� �������� �����������
                HandToTargetPosition(_secondaryHand);
                _secondaryHand = null;
                TwoHandGrab(true); // ���������� ������� �������, ��� ��� ��� ����� ���� �� ����� ����� ������ ��������� �� ������������ � ���������. ��� ��������� �� ��������� ��� ����� ����� �������-�������
            }
            else if (interactor == _secondaryHand) // ���� ��������� ������ �� ����� ����
            {
                HandToStartPosition(_secondaryHand); // ������ � ��������� ������� � ���������� �������
                TwoHandGrab(true);
                _secondaryHand = null;
            }
            SetRigidbodyDumping(70f);
        }
        else // ��� �������������� ���� ���������� ����� �� ����, �� ���� ����� ����� ���� � ���� �� ����� ��������
        {
            TwoHandGrab(true);
            HandToStartPosition(_firstHand);            
            _firstHand = null;
            grabInteractable._firstInteractor = null;
            grabInteractable.handModel = null;
            interactable.SetParent(null);
            SetRigidbodyDumping(0f);
        }        
    }
    public virtual void TwoHandGrab(bool value)
    {
        grabInteractable.trackPosition = value;
    }
    public virtual void SetRigidbodyDumping(float value)
    {
        rb.angularDamping = value;
        rb.linearDamping = value;
    }
    public virtual void SendHandModel(Transform hand)
    {
        grabInteractable._firstInteractor = hand;
        if (hand.CompareTag("L_Hand"))
        {
            grabInteractable.handModel = _rightHand;
        }
        else if (hand.CompareTag("R_Hand"))
        {
            grabInteractable.handModel = _leftHand;
        }
        else
        {
            return;
        }        
    }
    public virtual void HandToStartPosition(Transform hand)
    {
        if (hand == null) return; 
        if (hand.CompareTag("L_Hand"))
        {
            if (_leftHand == null) return; 
            if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = true; }            
            _leftHand.transform.SetParent(hand);
            _leftHand.transform.localPosition = Vector3.zero;
            _leftHand.transform.localRotation = Quaternion.Euler(-180, 167.888f, -90);
        }
        else if (hand.CompareTag("R_Hand"))
        {
            if (_rightHand == null) return; 
            if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = true; }
            _rightHand.transform.SetParent(hand);
            _rightHand.transform.localPosition = Vector3.zero;
            _rightHand.transform.localRotation = Quaternion.Euler(-180, 187.945f, 90);
        }
    }
    public virtual void HandToTargetPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            if (_leftHand == null) return;
            if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; }
            _leftHand.transform.SetParent(interactable);
            _leftHand.transform.localPosition = _leftHandPosition;
            _leftHand.transform.localRotation = _leftHandRotation;
        }
        else if (hand.CompareTag("R_Hand"))
        {
            if (_rightHand == null) return;
            if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
            _rightHand.transform.SetParent(interactable);
            _rightHand.transform.localPosition = _rightHandPosition;
            _rightHand.transform.localRotation = _rightHandRotation;
        }
    }
    protected virtual void Update()
    {
        if (_firstHand != null && _secondaryHand != null)
        {            
            Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, _trackPoint);           
          
            rb.AddForce((midPoint - transform.position) * 10f, ForceMode.VelocityChange);
        }
    }

    protected IEnumerator EnableCollision(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }   
}

