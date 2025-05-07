using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class GrabForInventory : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    private Item item;
    private Slot currentSlot;

    protected override void Awake()
    {
        base.Awake();
        item = GetComponent<Item>();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);

        if (item == null) return;
        if (!item.inSlot)
        {
            currentSlot = item.currentSlot;
            if (currentSlot != null)
            {
                currentSlot.ItemInSlot = null;
            }
            transform.parent = null; // **Убираем родителя**
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // **Включаем физику**
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

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

    }

    System.Collections.IEnumerator EnableCollision(float duration)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSeconds(duration);
        col.enabled = true;
    }
}

