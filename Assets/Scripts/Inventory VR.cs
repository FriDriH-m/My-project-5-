using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class InventoryVR : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Anchor;
    private bool UIActive;

    private void Start()
    {
        Inventory.SetActive(false);
        UIActive = false;
    }

    private void Update()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed) && isPressed)
        {
            UIActive = !UIActive;
            Inventory.SetActive(UIActive);
            Debug.Log("Кнопка нажата!");
        }

        if (UIActive)
        {
            Inventory.transform.position = Anchor.transform.position;
            Inventory.transform.eulerAngles = new Vector3(
                Anchor.transform.eulerAngles.x + 15,
                Anchor.transform.eulerAngles.y,
                0
            );
        }
    }
}