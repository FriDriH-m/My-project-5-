using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.Audio;

public class GrabForInventory : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    private Item item;
    private Slot currentSlot;
    [SerializeField] AudioClip _selectItem;
    [SerializeField] AudioSource _audioSource;
    protected override void Awake()
    {
        base.Awake();
        item = GetComponent<Item>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (item == null) return;

        Debug.Log("OnSelectEntering: Объект захвачен!");

        if (item.inSlot)
        {
            _audioSource.PlayOneShot(_selectItem);
            Debug.Log("OnSelectEntering: Объект вышел из слота!");
            currentSlot = item.currentSlot;
            if (currentSlot != null)
            {
                Debug.Log("OnSelectEntering: Слот найден!");
                currentSlot.ItemInSlot = null;
            }

            transform.parent = null; // Убираем родителя
            Debug.Log("OnSelectEntering: Родитель убран!");
            Debug.Log("OnSelectEntering: transform.parent = " + transform.parent);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("OnSelectEntering: Rigidbody найден!");
                rb.isKinematic = false; // Включаем физику
            }
            Debug.Log("OnSelectEntering: Физика включена!");

            item.inSlot = false;

            if (currentSlot != null)
            {
                Debug.Log("OnSelectEntering: Слот найден!");
                currentSlot.ResetColor();
            }
            item.currentSlot = null;
            StartCoroutine(EnableCollision(0.1f));
        }
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        Debug.Log("OnSelectExiting: Объект отпущен!");
        if (item != null && item.inSlot)
        {
            Debug.Log("OnSelectExiting: Объект все еще в слоте!");
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Debug.Log("OnSelectExiting: Rigidbody найден!");
            rb.isKinematic = false; // Включаем физику
        }
        Debug.Log("OnSelectExiting: Физика включена!");

        transform.parent = null; // Убираем родителя
        Debug.Log("OnSelectExiting: Родитель убран!");
        Debug.Log("OnSelectExiting: transform.parent = " + transform.parent);
    }

    System.Collections.IEnumerator EnableCollision(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }
}