using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.Audio;

public class GrabForInventory : MonoBehaviour
{
    private Item item;
    private Slot currentSlot;
    [SerializeField] AudioClip _selectItem;
    [SerializeField] AudioSource _audioSource;
    protected void Awake()
    {
        item = GetComponent<Item>();

    }

    protected void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (item == null) return;

        if (item.inSlot)
        {
            _audioSource.PlayOneShot(_selectItem);
            currentSlot = item.currentSlot;
            if (currentSlot != null)
            {
                currentSlot.ItemInSlot = null;
            }

            transform.parent = null; // Убираем родителя

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
    }

    protected void OnSelectExiting(SelectExitEventArgs args)
    {
        if (item != null && item.inSlot)
        {
            return;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Включаем физику
        }

        //transform.parent = null; // Убираем родителя
    }

    System.Collections.IEnumerator EnableCollision(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }

}



