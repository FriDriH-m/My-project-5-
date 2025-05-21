using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] Vector3 _offset; // ќт ќлега
    [SerializeField] LayerMask _layerMask; // ќт ќлега
    [SerializeField] private Transform _leftHand; // модельк левой руки
    [SerializeField] private Transform _rightHand; // моделька правой руки
    [SerializeField] private bool _canHoldTwoHands; // можно ли держать оружие двум€ руками

    [SerializeField] private Vector3 _leftHandPosition;  //
    [SerializeField] private Vector3 _leftHandEuler;     //  позиционирование моделек рук на руко€тке оружи€ после граба
    [SerializeField] private Vector3 _rightHandPosition; //
    [SerializeField] private Vector3 _rightHandEuler;    //
    [SerializeField] private Vector3 _twoHandEuler; // вращение в двуручном хвате
    [SerializeField] private float _trackPoint = 0.5f;
    private Quaternion _leftHandRotation => Quaternion.Euler(_leftHandEuler); // преобразование Vector3 в Quaternion и запись в переменную
    private Quaternion _rightHandRotation => Quaternion.Euler(_rightHandEuler); // преобразование Vector3 в Quaternion и запись в переменную
    private XRGrabInteractable grabInteractable;
    private Transform _firstHand; // переменна€ котора€ будет хранить кака€ рука схватила первой оружие€
    private Transform _secondaryHand; // переменна€ котора€ будет хранить кака€ рука схватила второй оружие
    Rigidbody rb;
    Transform interactable;
    Transform interactor;
    private Item item;
    private Slot currentSlot;
    [SerializeField] AudioClip _selectItem;
    [SerializeField] AudioSource _audioSource;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        item = GetComponent<Item>();
    }

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = _offset;
    }
    private void Start()
    {
        if (!_canHoldTwoHands)
        {
            grabInteractable.selectMode = InteractableSelectMode.Single;
            grabInteractable.focusMode = InteractableFocusMode.Single;
        }
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        // Ќачало твоего скрипта, »ль€
        if (item != null && item.inSlot)
        {
            _audioSource.PlayOneShot(_selectItem);
            currentSlot = item.currentSlot;
            if (currentSlot != null)
            {
                currentSlot.ItemInSlot = null;
            }
            // начало части моего скрипта
            if (_firstHand == null)
            {
                _firstHand = interactor;
                interactable.SetParent(interactor); // задаетс€ родитель в виде Near-Far Interactore в соответсвующем контроллере
                HandToTargetPosition(_firstHand);   // метод задает позицию дл€ рук
            }
            // конец части моего скрипта

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // ¬ключаем физику
            }

            item.inSlot = false;

            if (currentSlot != null)
            {
                currentSlot.ResetColor();
            }
            item.currentSlot = null;
            StartCoroutine(EnableCollision(0.1f));
        }
        // онец твоего скрипта
        if (_firstHand == null)
        {
            _firstHand = interactor;
            interactable.SetParent(interactor); // задаетс€ родитель в виде Near-Far Interactore в соответсвующем контроллере
            HandToTargetPosition(_firstHand);
            SetRigidbodyDumping(70f);
        }
        if (_canHoldTwoHands)
        {
            if (interactor.transform != _firstHand)
            {
                _secondaryHand = interactor;
                if (_firstHand.CompareTag("R_Hand"))
                {
                    if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; }
                    _leftHand.transform.SetParent(interactable);
                    _leftHand.transform.localPosition = _leftHandPosition - new Vector3(0, 0.2f, 0);
                    _leftHand.transform.localRotation = _leftHandRotation;
                }
                else
                {
                    if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
                    _rightHand.transform.SetParent(interactable);
                    _rightHand.transform.localPosition = _rightHandPosition - new Vector3(0, 0.2f, 0);
                    _rightHand.transform.localRotation = _rightHandRotation;
                }
                TwoHandGrab(false);
                SetRigidbodyDumping(40f);
            }
        }   
    }

    public void OnUngrab(SelectExitEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // ¬ключаем физику
        }

        if (_firstHand != null && _secondaryHand != null) // если при отпускании оружие вз€то обеими руками
        {
            if (interactor == _firstHand) // если отпускает перва€ рука, котора€ вз€лась за оружие
            {
                HandToStartPosition(_firstHand); // задаетс€ начальна€ позици€, то есть на –одину депортируют :,(
                _firstHand = _secondaryHand; // _firstHand принимает значение _secondaryHand чтобы в дальнейшем если отпущенной рукой возьметс€ игрок не было базара
                interactable.SetParent(_secondaryHand); // моделька руки становитс€ дочерним объектом контроллера
                HandToTargetPosition(_secondaryHand);
                _secondaryHand = null;
                TwoHandGrab(true); // включаетс€ трекинг позиции, так как дл€ одной руки он нужен чтобы оружие следовало за контроллером и крутилось. ѕри двуручном мы отключаем это чтобы самим крутить-вертеть
            }
            else if (args.interactorObject.transform == _secondaryHand) // если отпускает втора€ по счету рука
            {
                HandToStartPosition(_secondaryHand); // просто в начальную позицию и включаетс€ трекинг
                TwoHandGrab(true);
                _secondaryHand = null;
            }
            SetRigidbodyDumping(70f);
        }
        else // это обрабатываетс€ если двуручного хвата не было, то есть одной рукой вз€л и этой же рукой отпустил
        {
            TwoHandGrab(true);
            HandToStartPosition(_firstHand);            
            _firstHand = null;
            interactable.SetParent(null);
            SetRigidbodyDumping(0f);
        }        
    }
    public void TwoHandGrab(bool value)
    {
        grabInteractable.trackPosition = value;
        //grabInteractable.trackRotation = value;
    }
    public void SetRigidbodyDumping(float value)
    {
        rb.angularDamping = value;
        rb.linearDamping = value;
    }
    public void HandToStartPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = true; }            
            _leftHand.transform.SetParent(hand);
            _leftHand.transform.localPosition = Vector3.zero;
            _leftHand.transform.localRotation = Quaternion.Euler(-180, 167.888f, -90);
        }
        else if (hand.CompareTag("R_Hand"))
        {
            if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = true; }
            _rightHand.transform.SetParent(hand);
            _rightHand.transform.localPosition = Vector3.zero;
            _rightHand.transform.localRotation = Quaternion.Euler(-180, 187.945f, 90);
        }
    }
    public void HandToTargetPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; }
            _leftHand.transform.SetParent(interactable);
            _leftHand.transform.localPosition = _leftHandPosition;
            _leftHand.transform.localRotation = _leftHandRotation;
        }
        else if (hand.CompareTag("R_Hand"))
        {
            if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
            _rightHand.transform.SetParent(interactable);
            _rightHand.transform.localPosition = _rightHandPosition;
            _rightHand.transform.localRotation = _rightHandRotation;
        }
    }
    private void Update()
    {
        if (_firstHand != null && _secondaryHand != null)
        {
            
            Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, _trackPoint);
            Vector3 direction = _firstHand.position - _secondaryHand.position;

            // »спользуй физические методы дл€ перемещени€
            rb.AddForce((midPoint - transform.position) * 10, ForceMode.VelocityChange);
            //rb.AddTorque((direction - transform.eulerAngles) , ForceMode.VelocityChange);
        }
    }
    
    private IEnumerator EnableCollision(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }

    
}

