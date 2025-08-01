using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

public class Slot : MonoBehaviour
{
    public GameObject ItemInSlot;
    public Image slotImage;
    public Color originalColor;
    [SerializeField] AudioClip _selectItem;
    [SerializeField] AudioSource _audioSource;
    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {

        if (!other.CompareTag("Item")) return;
        GameObject obj = other.transform.parent.gameObject;
        if (ItemInSlot != null) return;  // ���� ���� ��� �����, ������ �� ������
        if (!IsItem(obj)) return;       // ���� ��� �� �������, ������ �� ������
        Item item = obj.GetComponent<Item>();                // �������� InsertItem, ����� ������� ������ � �������
        InsertItem(obj);

    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>() != null;
    }

    void InsertItem(GameObject obj)
    {
        Item item = obj.GetComponent<Item>();
        // ���������, ��� ������ �� �������� (�������)
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            // ���� ������ ��������, ������� �� �������
            return;
        }

        // ���� ����� ����, ������, ������� �� ��������, � ����� ��������� ��� � ����
        // �������� ������ ���������� �����
        _audioSource.PlayOneShot(_selectItem);
        // 1. ��������� ����������� ������ (���� ��� �� ��������)
        if (item.originalScale == Vector3.zero)
            item.originalScale = obj.transform.localScale;

        // 2. ������������ ��� ������ �����
        obj.transform.localScale *= item.ScaleInSlot;
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = item.RotationInSlot;
        obj.transform.localPosition += item.PositionOffset;
        obj.GetComponent<Item>().inSlot = true;
        obj.GetComponent<Item>().currentSlot = this;
        ItemInSlot = obj;
        slotImage.color = Color.gray;
    }


    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}