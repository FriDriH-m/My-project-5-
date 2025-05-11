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

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if (ItemInSlot != null) return;  // Если слот уже занят, ничего не делаем
        if (!IsItem(obj)) return;       // Если это не предмет, ничего не делаем
        // Вызываем InsertItem, когда предмет входит в триггер
        InsertItem(obj);
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>() != null;
    }

    void InsertItem(GameObject obj)
    {
        // Проверяем, что объект не захвачен (отпущен)
        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            // Если объект захвачен, выходим из функции
            return;
        }
        // Если дошли сюда, значит, предмет не захвачен, и можно поместить его в слот
        // Получаем размер коллайдера слота
        _audioSource.PlayOneShot(_selectItem);
        Collider slotCollider = GetComponent<Collider>();
        Vector3 slotSize = slotCollider.bounds.size;

        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;
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
