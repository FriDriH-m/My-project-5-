using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item : MonoBehaviour
{
    public bool inSlot; // Точь-в-точь как у гайдера
    public Vector3 slotRotation = Vector3.zero; // Без изменений
    public Slot currentSlot; // Как в оригинале

    private void Awake()
    {
        // Автоматически добавляем XRGrabInteractable, если его нет
        var grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();

        // Подписываемся на событие захвата (аналог GrabBegin)
        grabInteractable.selectEntered.AddListener(OnGrabbed);
    }

    // Дословный перенос логики из GrabBegin
    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Часть 1: Делаем объект кинематическим (как в оригинале)
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        // Часть 2: Код гайдера без изменений
        if (GetComponent<Item>() == null) return; // Проверка на Item
        if (inSlot)
        {
            GetComponentInParent<Slot>().ItemInSlot = null;
            transform.parent = null;
            inSlot = false;
            currentSlot.ResetColor();
            currentSlot = null;
        }
    }
}