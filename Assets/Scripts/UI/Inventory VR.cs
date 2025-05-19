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
    private bool _wasPressed = false;
    [SerializeField] AudioClip _openAndCloseEffect;
    [SerializeField] AudioSource _audioSource;
    private void Awake()
    {
        Inventory.SetActive(false);
        UIActive = false;
    }

    private void Update()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed))
        {
            if (isPressed && !_wasPressed)
            {
                UIActive = !UIActive;
                Inventory.SetActive(UIActive);
                _audioSource.PlayOneShot(_openAndCloseEffect);
            }

            if (!isPressed)
            {
                _wasPressed = false;
            }
            else
            {
                _wasPressed = true;
            }
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