using UnityEngine;
using UnityEngine.XR;

public class VRHandTracking : MonoBehaviour
{
    [Header("VR Controllers")]
    public XRNode leftHandNode = XRNode.LeftHand; // Левый контроллер
    public XRNode rightHandNode = XRNode.RightHand; // Правый контроллер

    [Header("Model Bones")]
    public Transform leftHandBone; // Кость левой руки модели
    public Transform rightHandBone; // Кость правой руки модели

    void Update()
    {
        UpdateHandPosition(leftHandNode, leftHandBone);
        UpdateHandPosition(rightHandNode, rightHandBone);
    }

    void UpdateHandPosition(XRNode node, Transform bone)
    {
        if (bone == null) return;

        // Получаем данные VR-устройства
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        // Если устройство найдено, обновляем позицию и вращение
        if (device.isValid)
        {
            // Позиция и вращение контроллера
            device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
            device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

            // Применяем к кости модели
            bone.position = position;
            bone.rotation = rotation;
        }
    }
}
