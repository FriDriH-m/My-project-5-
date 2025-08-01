using GLTFast.Schema;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.Audio;

public class BowGrabParenter : GrabParenter
{
    Animator animator; // �������� ����
    public bool isGrabbed; // ���� �� ����� ��� � ����. � GrabParenter �������� true
    [SerializeField] GameObject arrowModel;
    [SerializeField] Transform firstPoint;
    [SerializeField] Transform secondPoint;
    [SerializeField] GameObject tetiva; // ������ ����
    [SerializeField] GameObject arrow; // ������ ������ ����� ��������� 
    private AudioSource audioSource;
    public AudioClip[] hitArrowSounds;
    float animationAmount;
    GameObject spawnedArrow; // ��������� ������
    Rigidbody arrowRb;
    public int Shoots;
    [SerializeField] Transform spawnPoint; // ����� ������ ������
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    protected override void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
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
            //Debug.Log($"currentDist = {animationAmount}");
            // ������ ����� ��������� - ����������������, ��������
            Vector3 vector = secondPoint.position - firstPoint.position; 
            vector.Normalize();
            Quaternion baseRotation = Quaternion.LookRotation(vector);
            Quaternion correction = Quaternion.Euler(90f, 0f, 2.5f);
            arrowModel.transform.rotation = baseRotation * correction;

            animator.Play("Bow", 0, animationAmount);

            //Vector3 midPoint = Vector3.Lerp(_firstHand.position, _secondaryHand.position, _trackPoint) - new Vector3 (0f, 0.25f, 0.2f);
            //rb.AddForce((midPoint - (transform.position + new Vector3(0, 0, 0))) * 10, ForceMode.VelocityChange);
            //rb.AddTorque(_firstHand.position - _secondaryHand.position, ForceMode.VelocityChange);
        }
    }

    public override void OnGrab(SelectEnterEventArgs args)
    {
        interactable = args.interactableObject.transform;
        interactor = args.interactorObject.transform;
        // ������ ������ �������, ����
        if (item != null && item.inSlot)
        {
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
        //����� ������ �������
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

            Vector3 spawnPosition = spawnPoint.position;
            Vector3 direction = (_leftHand.position - _rightHand.position).normalized;

            spawnedArrow = Instantiate(arrow, spawnPosition, Quaternion.LookRotation(transform.up) * Quaternion.LookRotation(transform.forward) * Quaternion.LookRotation(transform.right));
            //spawnedArrow.transform.rotation = ;
            //spawnedArrow.transform.Rotate(0, 0, -90);
            StartCoroutine(DeleteArrow(spawnedArrow));

            arrowRb = spawnedArrow.GetComponent<Rigidbody>();
            spawnedArrow.transform.SetParent(null);
            arrowRb.AddForce(transform.right * 23 * animationAmount, ForceMode.Impulse);
            animator.Play("Bow", 0, 0);
            HitArrowSound();
            Shoots += 1;
        }
        base.OnUngrab(args);
    }

    public void HitArrowSound()
    {
        if (hitArrowSounds.Length > 0)
        {
            int randomSoundIndex = Random.Range(0, hitArrowSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);  // ���� ������ ���
            audioSource.PlayOneShot(hitArrowSounds[randomSoundIndex]);
        }
    }
    public override void TwoHandGrab(bool value)
    {
        //grabInteractable.trackPosition = value;
        //grabInteractable.trackRotation = value;   
    }
    public IEnumerator DeleteArrow(GameObject arrow)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(arrow);
    }
}
