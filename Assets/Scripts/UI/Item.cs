using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item : MonoBehaviour
{
    public bool inSlot;
    public Vector3 slotRotation = Vector3.zero;
    public Slot currentSlot;
    public Vector3 originalRotation;
    public Vector3 originalScale; // Новое: хранит изначальный размер
    [SerializeField] public float ScaleInSlot = 0.2f;
    [SerializeField] public Quaternion RotationInSlot = Quaternion.Euler(0f, 0f, 0f);
    [SerializeField] public Vector3 PositionOffset;

    void Start()
    {
        originalScale = transform.localScale; // Новое: запоминаем начальный масштаб
        originalRotation = transform.localEulerAngles;
    }
}