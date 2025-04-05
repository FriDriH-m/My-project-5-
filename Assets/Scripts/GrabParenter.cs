using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabParenter : MonoBehaviour
{
    [SerializeField] GameObject plane;

    public void OnGrab(SelectEnterEventArgs args)
    {
        args.interactableObject.transform.SetParent(args.interactorObject.transform);
        if (args.interactableObject.transform.gameObject.CompareTag("Weapon"))
        {
            plane.SetActive(true);
        }
    }
    public void OnUngrab(SelectExitEventArgs args)
    {
        args.interactableObject.transform.SetParent(null);
    }
}
