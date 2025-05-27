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
        Item item = obj.GetComponent<Item>();
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
        // 1. Сохраняем изначальный размер (если еще не сохранен)
        if (item.originalScale == Vector3.zero)
            item.originalScale = obj.transform.localScale;

        // 2. Масштабируем под размер слота
        if (TryGetComponent<BoxCollider>(out var slotCollider))
        {
            Vector3 slotSize = slotCollider.size; // Размер слота
            Vector3 objectSize = obj.GetComponent<Renderer>().bounds.size; // Размер предмета

            // Вычисляем коэффициент масштабирования
            float scaleRatio = Mathf.Min(
                slotSize.x / objectSize.x,
                slotSize.y / objectSize.y,
                slotSize.z / objectSize.z
            ) * 0.8f; // 0.8 — небольшой отступ от краев

            // Применяем новый размер
            obj.transform.localScale = item.originalScale * scaleRatio;
        }

        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;
        obj.GetComponent<Item>().inSlot = true;
        obj.GetComponent<Item>().currentSlot = this;
        ItemInSlot = obj;
        slotImage.color = Color.gray;
    }

    public void RemoveItem()
    {
        ItemInSlot.transform.localScale = ItemInSlot.GetComponent<Item>().originalScale;
    }

    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}