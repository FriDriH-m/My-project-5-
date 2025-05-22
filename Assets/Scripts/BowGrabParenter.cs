using GLTFast.Schema;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class BowGrabParenter : GrabParenter
{
    Animator animator; // аниматор лука
    public bool isGrabbed; // взял ли игрок лук в руки. в GrabParenter задается true
    [SerializeField] GameObject arrowModel;
    [SerializeField] Transform firstPoint;
    [SerializeField] Transform secondPoint;
    [SerializeField] GameObject tetiva; // тетива лука
    [SerializeField] GameObject arrow; // префаб стрелы чтобы создавать 
    float animationAmount;
    GameObject spawnedArrow; // созданная стрела
    Rigidbody arrowRb;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    protected override void Update()
    {

        if (_secondaryHand != null && _firstHand != null)
        {
            arrowModel.SetActive(true);
            float maxPull = 0.6f;
            float currentDist = Vector3.Distance(_firstHand.position, _secondaryHand.position);            

            animationAmount = Mathf.Clamp01(currentDist / maxPull);
            if (animationAmount > 0.96f)
            {
                animationAmount = 0.96f;
            }
            Vector3 vector = secondPoint.position - firstPoint.position;
            vector.Normalize();
            Quaternion baseRotation = Quaternion.LookRotation(vector);
            Quaternion correction = Quaternion.Euler(90f, 0f, 2.5f);
            arrowModel.transform.rotation = baseRotation * correction;
            animator.Play("Bow", 0, animationAmount);

            Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, _trackPoint) - new Vector3 (0, 0.25f, 0);
            rb.AddForce((midPoint - transform.position) * 10, ForceMode.VelocityChange);
        }
    }
    public override void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        // Начало твоего скрипта, Илья
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
                interactable.SetParent(interactor); // задается родитель в виде Near-Far Interactore в соответсвующем контроллере
                HandToTargetPosition(_firstHand);   // метод задает позицию для рук
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
            SetRigidbodyDumping(70f);
        }
        if (_canHoldTwoHands)
        {
            if (interactor.transform != _firstHand)
            {
                interactable = tetiva.transform;
                _secondaryHand = interactor;
                if (_firstHand.CompareTag("R_Hand"))
                {
                    if (_leftHand.GetComponent<Collider>() != null) { _leftHand.GetComponent<Collider>().enabled = false; }
                    _leftHand.transform.SetParent(interactable);
                    _leftHand.transform.localPosition = new Vector3 (0.00331f, -0.00094f, -0.00021f);
                    _leftHand.transform.localRotation = Quaternion.Euler(73.541f, -128.373f, -49.61f);
                }
                else
                {
                    if (_rightHand.GetComponent<Collider>() != null) { _rightHand.GetComponent<Collider>().enabled = false; }
                    _rightHand.transform.SetParent(interactable);
                    _rightHand.transform.localPosition = Vector3.zero;
                    _rightHand.transform.localRotation = Quaternion.Euler(75.877f, 9.717f, -85.261f);
                }
                TwoHandGrab(false);
                SetRigidbodyDumping(40f);
            }
        }
    }
    public override void OnUngrab(SelectExitEventArgs args)
    {
        if (_firstHand != null && _secondaryHand != null)
        {
            arrowModel.SetActive(false);

            Vector3 spawnPosition = transform.position + transform.right * 0.4f + transform.up * 0.3f;
            spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);
            spawnedArrow.transform.rotation = Quaternion.LookRotation(transform.up);

            StartCoroutine(DeleteArrow(spawnedArrow));

            arrowRb = spawnedArrow.GetComponent<Rigidbody>();
            spawnedArrow.transform.SetParent(null);
            Debug.Log(animationAmount + " импульс - " + (30 * animationAmount));
            arrowRb.AddForce(transform.right * 33 * animationAmount, ForceMode.Impulse);
            animator.Play("Bow", 0, 0);
        }
        base.OnUngrab(args);
    }
    public IEnumerator DeleteArrow(GameObject arrow)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(arrow);
    }
}
