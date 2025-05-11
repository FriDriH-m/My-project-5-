using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class GrabParenter : MonoBehaviour
{
    [SerializeField] Vector3 _offset; // От Олега
    [SerializeField] LayerMask _layerMask; // От Олега
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
    public void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;\
        // Начало твоего скрипта, Илья
        if (item.inSlot && item != null)
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
                interactable.SetParent(interactor); // задается родитель в виде Near-Far Interactore в соответсвующем контроллере
                HandToTargetPosition(_firstHand);   // метод задает позицию для рук
            }
            if (_canHoldTwoHands) // если оружие можно держать двумя руками (одноручное нельзя будет). Указывается в инспекторе
            {
                if (interactor.transform != _firstHand) // если берешь той рукой, которой до этого не брал. _firstHand принимает значение при первом взятии, всегда.
                {
                    _secondaryHand = interactor;
                    if (_firstHand.CompareTag("R_Hand")) // вычисляется тег второй руки. Если у _firstHand тег R_Hand, то у второго L_Hand :O (ибануться можно).
                    {
                        if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; } // отключает коллайдер чтобы не мешало
                        _leftHand.transform.SetParent(interactable); // родителем модельки руки становится меч
                        _leftHand.transform.localPosition = _leftHandPosition - new Vector3(0, 0.2f, 0); // просто позиционирование
                        _leftHand.transform.localRotation = _leftHandRotation;
                    }
                    else // начинка та же
                    {
                        if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
                        _rightHand.transform.SetParent(interactable);
                        _rightHand.transform.localPosition = _rightHandPosition - new Vector3(0, 0.2f, 0);
                        _rightHand.transform.localRotation = _rightHandRotation;
                    }
                    TwoHandGrab(false);
                }
            }
            // конец части моего скрипта

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Включаем физику
            }

            item.inSlot = false;

            if (currentSlot != null)
            {
                currentSlot.ResetColor();
            }
            item.currentSlot = null;
            StartCoroutine(EnableCollision(0.1f));
        }
        //Конец твоего скрипта
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
            }
        }   
        //rb.linearDamping = 70f;
        //rb.angularDamping = 70f;
    }

    public void OnUngrab(SelectExitEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Включаем физику
        }

        if (_firstHand != null && _secondaryHand != null) // если при отпускании оружие взято обеими руками
        {
            if (args.interactorObject.transform == _firstHand) // если отпускает первая рука, которая взялась за оружие
            {
                HandToStartPosition(_firstHand); // задается начальная позиция, то есть на Родину депортируют :,(
                _firstHand = _secondaryHand; // _firstHand принимает значение _secondaryHand чтобы в дальнейшем если отпущенной рукой возьмется игрок не было базара
                interactable.SetParent(interactor); // моделька руки становится дочерним объектом контроллера
                _secondaryHand = null;
                TwoHandGrab(true); // включается трекинг позиции, так как для одной руки он нужен чтобы оружие следовало за контроллером и крутилось. При двуручном мы отключаем это чтобы самим крутить-вертеть
            }
            else if (args.interactorObject.transform == _secondaryHand) // если отпускает вторая по счету рука
            {
                HandToStartPosition(_secondaryHand); // просто в начальную позицию и включается трекинг
                TwoHandGrab(true);
                _secondaryHand = null;
            }
        }
        else // это обрабатывается если двуручного хвата не было, то есть одной рукой взял и этой же рукой отпустил
        {
            TwoHandGrab(true);
            HandToStartPosition(_firstHand);            
            _firstHand = null;
            interactable.SetParent(null);
        }        
    }
    public void TwoHandGrab(bool value)
    {
        grabInteractable.trackPosition = value;
        grabInteractable.trackRotation = value;
    }
    public void HandToStartPosition(Transform hand)
    {
        if (hand.CompareTag("L_Hand"))
        {
            if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = true; }            
            _leftHand.transform.SetParent(interactor);
            _leftHand.transform.localPosition = Vector3.zero;
            _leftHand.transform.localRotation = Quaternion.Euler(-180, 167.888f, -90);
        }
        else if (hand.CompareTag("R_Hand"))
        {
            if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = true; }
            _rightHand.transform.SetParent(interactor);
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
            Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, 0.5f);
            transform.position = midPoint;

            Vector3 direction = _firstHand.position - _secondaryHand.position;
            transform.rotation = Quaternion.LookRotation(direction) * _twoHandRotation;
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

