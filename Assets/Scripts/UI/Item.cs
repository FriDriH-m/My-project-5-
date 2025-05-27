using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item : MonoBehaviour
{
    public bool inSlot;
    public Vector3 slotRotation = Vector3.zero;
    public Slot currentSlot;
    public Vector3 originalScale; // Новое: хранит изначальный размер

    void Start()
    {
        originalScale = transform.localScale; // Новое: запоминаем начальный масштаб
    }
}